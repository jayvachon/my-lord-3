﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Building : Clickable
{

    public float startingValue = 100000;
    public Material ownedMaterial;
    public Material unownedMaterial;
    public Material renovatingMaterial;
    public GameObject attention;

	bool owned = false;
    bool hasTenants = true;
    bool tenantsPayingRent = true;

    int currentMonth = 1;

    public bool EvictionOrder { get; private set; }
    public bool Selected { get; private set; }
    public bool NeedsRepair { get; private set; }

	public float PropertyValue {
		get { 
            return startingValue * BuildingManager.valueMultiplier; 
        }
	}

    public float RenovationCost {
        get { return startingValue * 0.2f; }
    }

    int renovationStart = 1;
    public bool Renovating { get; private set; }

    public bool Owned {
        get { return owned; }
    }

    public bool HasTenants {
        get { return hasTenants; }
    }

    float Rent {
        get { return startingValue * 0.04f; }
    }

    Vector3 startingScale;

    void Start() {
        startingScale = transform.localScale;
        attention.gameObject.SetActive(false);
    }

    public void Init(BuildingConfig b) {
        transform.localScale = new Vector3(transform.localScale.x, b.height, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, b.height / 2f, transform.position.z);
        startingScale = transform.localScale;
        startingValue = b.value;
    }

    void Update() {

    	// Buy
    	if (Input.GetKeyDown(KeyCode.B)) {
    		if (Selected && Purse.wealth >= PropertyValue) {
    			Buy();
    		}
    	}

        if (owned) {

            if (Selected) {

            	// Sell
            	if (Input.GetKeyDown(KeyCode.S)) {
                    if (!Renovating) Sell();
            	}

                // Evict
                if (Input.GetKeyDown(KeyCode.E)) {
                    if (hasTenants) {
                        Evict();
                    }
                }

                // Lease
                if (Input.GetKeyDown(KeyCode.L)) {
                    if (!hasTenants) {
                        Debug.Log("Leased to tenants");
                        tenantsPayingRent = true;
                        hasTenants = true;
                    }
                }

                // Renovate
                if (Input.GetKeyDown(KeyCode.R)) {
                    if (!hasTenants && Purse.wealth >= RenovationCost) {
                        Debug.Log("Renovating");
                        Purse.wealth -= RenovationCost;
                        renovationStart = currentMonth;
                        Renovating = true;
                        GetComponent<MeshRenderer>().material = renovatingMaterial;
                    }
                }

                // Fix
                if (Input.GetKeyDown(KeyCode.F)) {
                    if (NeedsRepair && Purse.wealth >= 100) {
                        Debug.Log("Fix");
                        Purse.wealth -= 100;
                        NeedsRepair = false;
                        attention.gameObject.SetActive(false);                       
                    }
                }
            }
        }
    }

    void Buy() {
        Debug.Log("Bought for " + PropertyValue);
        owned = true;
        Purse.wealth -= PropertyValue;
        GetComponent<MeshRenderer>().material = ownedMaterial;
    }

    void Sell() {
        Debug.Log("Sold for " + PropertyValue);
        owned = false;
        Purse.wealth += PropertyValue;
        GetComponent<MeshRenderer>().material = unownedMaterial;
    }

    void Evict() {
        if (Random.value >= 0.5f) {
            Debug.Log("Tenants refuse to leave");
            tenantsPayingRent = false;
            EvictionOrder = true;
        } else {
            CompleteEviction();
        }
    }

    void CompleteEviction() {
        Debug.Log("Tenants evicted");
        hasTenants = false;
        GameObjectPool.Instantiate("UnhousedPerson", new Vector3(0f, 0.26f, -1f));
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
        Events.instance.AddListener<CallPoliceEvent>(OnCallPoliceEvent);
    }

    void OnNewMonthEvent(NewMonthEvent e) {
        currentMonth = e.Month;
        if (owned && hasTenants && tenantsPayingRent) {
            
            // collect rent
            Purse.wealth += Rent;

            // there's a chance a repair is needed
            if (Random.value >= 0.8f) {
                NeedsRepair = true;
                attention.gameObject.SetActive(true);
            }
        }

        // renovation takes 6 months
        if (Renovating && (currentMonth - renovationStart > 6)) {
            Renovating = false;
            startingValue *= 1.5f;
            Debug.Log("Renovated! Value increased.");
            GetComponent<MeshRenderer>().material = ownedMaterial;
        }
    }

    void OnCallPoliceEvent(CallPoliceEvent e) {
        if (EvictionOrder) {
            CompleteEviction();
        }
    }
    #endregion
}
