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
        public GameObject tenantsUnionIndicator;
        public GameObject xIndicator;
        public GameObject crown;

    	bool owned = false;
        bool hasTenants = true;
        bool tenantsPayingRent = true;
        bool isTenantsUnionHeadquarters = false;
        bool renovated = false; // Only one renovation is permitted

        int currentMonth = 1;
        int ignoredRepairs = 0;
        int monthsRepairNeeded = 0;

        public bool EvictionOrder { get; private set; }
        public bool RaiseRentOrder { get; private set; }
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
            get { return (valueAtBuy * 0.01f).RoundToInterval(500); }
        }

        public bool HasRent {
            get { return HasTenants && tenantsPayingRent; }
        }

        float NewRent {
        	get { return (PropertyValue * 0.01f).RoundToInterval(500); }
        }

        public bool CanRaiseRent {
        	get { return !RaiseRentOrder && NewRent >  Rent; }
        }

        public bool CanRenovate {
        	get { return !renovated; }
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

        int repairCost = 1000;
        public int RepairCost {
        	get { return repairCost; }
        }

        Vector3 startingScale;

        void Start() {
            startingScale = transform.localScale;
            attention.gameObject.SetActive(false);
            tenantsUnionIndicator.gameObject.SetActive(false);
            xIndicator.gameObject.SetActive(false);
            crown.gameObject.SetActive(false);
        }

        public void Init(BuildingConfig b) {
            transform.localScale = new Vector3(transform.localScale.x, b.height, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, b.height / 2f, transform.position.z);
            startingScale = transform.localScale;
            startingValue = b.value;
        }

        public void SetAsHighestValue() {
        	Co.WaitForFixedUpdate(() => {
	        	crown.gameObject.SetActive(true);
    		});
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
                            StartRent();
                            hasTenants = true;
                            valueAtBuy = PropertyValue; // raise the rent automatically
                            Events.instance.Raise(new LeaseApartmentEvent(this));
                        }
                    }

                    // Renovate
                    if (Input.GetKeyDown(KeyCode.R)) {
                        if (!renovated && !hasTenants && Purse.wealth >= RenovationCost) {
                            Purse.wealth -= RenovationCost;
                            renovationStart = currentMonth;
                            Renovating = true;
                            NeedsRepair = false;
                            attention.gameObject.SetActive(false);
                            GetComponent<MeshRenderer>().material = renovatingMaterial;
                            Events.instance.Raise(new StartRenovationEvent(this));
                        }
                    }

                    // Fix
                    if (Input.GetKeyDown(KeyCode.F)) {
                        if (NeedsRepair && Purse.wealth >= RepairCost) {
                            Purse.wealth -= RepairCost;
                            NeedsRepair = false;
                            monthsRepairNeeded = 0;
                            attention.gameObject.SetActive(false);
                            StartRent();               
                        }
                    }

                    // Ignore repair
                    /*if (Input.GetKeyDown(KeyCode.C)) {
                    	if (NeedsRepair) {
                    		NeedsRepair = false;
                    		attention.gameObject.SetActive(false);
                    		ignoredRepairs ++;
                    	}
                    }*/

                    // Raise rent
                    if (Input.GetKeyDown(KeyCode.U)) {
                    	if (CanRaiseRent) {
                            if (Random.value >= 0.9f) {
                                RaiseRentOrder = true;
                                Events.instance.Raise(new RefuseRentIncreaseEvent(this));
                            } else {
                        		valueAtBuy = PropertyValue;
                            }
                    	}
                    }
                }
            }
        }

        public void MakeTenantsUnionHeadquarters() {
        	isTenantsUnionHeadquarters = true;
        	tenantsUnionIndicator.gameObject.SetActive(true);

        }

        void Buy() {
            valueAtBuy = PropertyValue;
            owned = true;
            Purse.wealth -= PropertyValue;
            GetComponent<MeshRenderer>().material = ownedMaterial;
            Events.instance.Raise(new BuyApartmentEvent(this));
        }

        void Sell() {
            owned = false;
            NeedsRepair = false;
            monthsRepairNeeded = 0;
            attention.gameObject.SetActive(false);
            Purse.wealth += PropertyValue;
            GetComponent<MeshRenderer>().material = unownedMaterial;
            xIndicator.gameObject.SetActive(false);
            Events.instance.Raise(new SellApartmentEvent(this));
        }

        void Evict() {
            
            StopRent();

            if (GameManager.Instance.GlobalRentStrike || EvictionOrder || Random.value < TenantUnion.Instance.ChanceOfEvictionRefusal) {
                EvictionOrder = true;
                Events.instance.Raise(new RefuseEvictionEvent(this));
            } else {
                CompleteEviction();
            }
        }

        void CompleteEviction() {
            EvictionOrder = false;
            hasTenants = false;
            GameObjectPool.Instantiate("UnhousedPerson", new Vector3(0f, 0.26f, -1f));
            Events.instance.Raise(new CompleteEvictionEvent());
        }

        void StopRent() {
        	tenantsPayingRent = false;
        	xIndicator.gameObject.SetActive(true);
        	attention.gameObject.SetActive(false);
        }

        void StartRent() {
        	tenantsPayingRent = true;
        	xIndicator.gameObject.SetActive(false);
        }

        protected override void OnSelect() {
            transform.localScale = new Vector3(startingScale.x * 1.1f, startingScale.y * 1.1f, startingScale.z * 1.1f);
        }

        protected override void OnDeselect() {
            transform.localScale = startingScale;
        }

        #region IClickable
        protected override void AddListeners() {
            base.AddListeners();
            Events.instance.AddListener<NewMonthEvent>(OnNewMonthEvent);
            Events.instance.AddListener<CallPoliceEvent>(OnCallPoliceEvent);
            Events.instance.AddListener<CourtCaseEvent>(OnCourtCaseEvent);
        }

        void OnNewMonthEvent(NewMonthEvent e) {
            currentMonth = e.Month;

            if (NeedsRepair) {
            	monthsRepairNeeded ++;
            	if (tenantsPayingRent) {

            		// The chances that tenants will stop paying rent until the apartment is fixed go up each month
	            	if (Random.value < ((float)monthsRepairNeeded / 36f)) {
	            		StopRent();
	            		Events.instance.Raise(new RentStrikeEvent(this));
	            	}
            	}
            }

            if (owned && hasTenants && tenantsPayingRent) {
                
                // collect rent
                Purse.wealth += Rent;

                // there's a chance a repair is needed
                if (!NeedsRepair && Random.value >= 0.95f) {
                    NeedsRepair = true;
                    repairCost = (int)((Random.value * 10000).RoundToInterval(500));
                    attention.gameObject.SetActive(true);
                }
            }

            // renovation takes 6 months
            if (Renovating && (currentMonth - renovationStart > 6)) {
                Renovating = false;
                startingValue *= 1.5f;
                ignoredRepairs = 0;
                renovated = true;
                GetComponent<MeshRenderer>().material = ownedMaterial;
                Events.instance.Raise(new EndRenovationEvent(this));
            }
        }

        void OnCallPoliceEvent(CallPoliceEvent e) {
            if (EvictionOrder) {
                if (Random.value >= 0.5f) {
                    CompleteEviction();
                } else {
                    Events.instance.Raise(new RefuseEvictionEvent(this));
                }
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
