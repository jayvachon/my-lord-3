using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Apartment : Building
{
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

        float startingValue = 100000;
        float valueAtBuy = 0; // the value of the property when it was bought
    	public override float PropertyValue {
    		get { 
                return (((startingValue + PropertyNeighborModifier) / 2) // Take average of property value in isolation and property value of neighbors
                	* BuildingManager.valueMultiplier) // All property values increase over time
	                .RoundToInterval(5000); // Round to the nearest $5000
            }
    	}

        public float Rent {
            get { return (valueAtBuy * 0.04f).RoundToInterval(1000); }
        }

        float NewRent {
        	get { return (PropertyValue * 0.04f).RoundToInterval(1000); }
        }

        public bool CanRaiseRent {
        	get { return NewRent >  Rent; }
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
                            valueAtBuy = PropertyValue; // raise the rent automatically
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

                    // Raise rent
                    if (Input.GetKeyDown(KeyCode.U)) {
                    	if (CanRaiseRent) {
                    		valueAtBuy = PropertyValue;
                    	}
                    }
                }
            }
        }

        void Buy() {
            Debug.Log("Bought for " + PropertyValue);
            valueAtBuy = PropertyValue;
            owned = true;
            Purse.wealth -= PropertyValue;
            GetComponent<MeshRenderer>().material = ownedMaterial;
            Events.instance.Raise(new BuyApartmentEvent(this));
        }

        void Sell() {
            Debug.Log("Sold for " + PropertyValue);
            owned = false;
            Purse.wealth += PropertyValue;
            GetComponent<MeshRenderer>().material = unownedMaterial;
            Events.instance.Raise(new SellApartmentEvent(this));
        }

        void Evict() {
            if (EvictionOrder || Random.value >= 0.5f) {
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
            Events.instance.Raise(new SelectApartmentEvent(this));
            Selected = true;
            transform.localScale = new Vector3(startingScale.x * 1.1f, startingScale.y * 1.1f, startingScale.z * 1.1f);
        }

        void Deselect() {
            Events.instance.Raise(new DeselectApartmentEvent());
            Selected = false;
            transform.localScale = startingScale;
        }
        #endregion

        #region IClickable
        protected override void AddListeners() {
            base.AddListeners();
            Events.instance.AddListener<NewMonthEvent>(OnNewMonthEvent);
            Events.instance.AddListener<CallPoliceEvent>(OnCallPoliceEvent);
            Events.instance.AddListener<CourtCaseEvent>(OnCourtCaseEvent);
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
            if (EvictionOrder && Random.value >= 0.5f) {
                CompleteEviction();
            } else {
                Debug.Log("Tenants have a lawyer and refuse to leave");
            }
        }

        void OnCourtCaseEvent(CourtCaseEvent e) {
            if (EvictionOrder) {
                float settlement = Rent * 4f;
                Purse.wealth -= settlement;
                Debug.Log("Case settled for $" + settlement.ToString());
                CompleteEviction();
            }
        }
        #endregion
}
