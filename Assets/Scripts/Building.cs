using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Building : Clickable
{

    public float startingValue = 100000;
    public Material ownedMaterial;
    public Material unownedMaterial;

	bool owned = false;
    bool hasTenants = true;
    bool tenantsPayingRent = true;

    public bool Selected { get; private set; }

	public float Value {
		get { return startingValue * BuildingManager.valueMultiplier; }
	}

    float Rent {
        get { return startingValue * 0.02f; }
    }

    Vector3 startingScale;

    void Start() {
        startingScale = transform.localScale;
    }

    void Update() {

    	// Buy
    	if (Input.GetKeyDown(KeyCode.B)) {
    		if (Selected && Purse.wealth >= Value) {
    			Buy();
    		}
    	}

        if (owned) {

            if (Selected) {

            	// Sell
            	if (Input.GetKeyDown(KeyCode.S)) {
        			Sell();
            	}

                // Evict
                if (Input.GetKeyDown(KeyCode.E)) {
                    if (hasTenants) {
                        Debug.Log("Tenants evicted");
                        hasTenants = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.L)) {
                    if (!hasTenants) {
                        Debug.Log("Leased to tenants");
                        hasTenants = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.R)) {
                    if (!hasTenants && Purse.wealth >= startingValue * 0.2f) {
                        Debug.Log("Renovated! Value increased.");
                        Purse.wealth -= startingValue * 0.2f;
                        startingValue *= 1.5f;
                    }
                }
            }
        }
    }

    void Buy() {
        Debug.Log("Bought for " + Value);
        owned = true;
        Purse.wealth -= Value;
        GetComponent<MeshRenderer>().material = ownedMaterial; 
    }

    void Sell() {
        Debug.Log("Sold for " + Value);
        owned = false;
        Purse.wealth += Value;
        GetComponent<MeshRenderer>().material = unownedMaterial;
    }

    #region Clickable
    public override void ClickThis() {
        Toggle();
    }

    public override void ClickOther() {
        if (Selected) {
            Deselect();
        }
    }

    void Toggle() {
        if (Selected) {
            Deselect();
        } else {
            Select();
        }
    }

    void Select() {

        // To avoid race condition between de/selecting, always select last
        StartCoroutine(CoSelect());
    }
    IEnumerator CoSelect() {
        yield return new WaitForEndOfFrame();
        Events.instance.Raise(new SelectBuildingEvent(this));
        Selected = true;
        transform.localScale = new Vector3(startingScale.x * 1.1f, startingScale.y * 1.1f, startingScale.z * 1.1f);
    }

    void Deselect() {
        Events.instance.Raise(new DeselectBuildingEvent());
        Selected = false;
        transform.localScale = startingScale;
    }
    #endregion

    #region IClickable
    protected override void AddListeners() {
        base.AddListeners();
        Events.instance.AddListener<NewMonthEvent>(OnNewMonthEvent);
    }

    void OnNewMonthEvent(NewMonthEvent e) {
        if (owned && hasTenants) {
            Purse.wealth += Rent;
        }
    }
    #endregion
}
