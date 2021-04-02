using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EventSystem;

public class BuildingManager: MB
{
    private static BuildingManager instance;
    public static BuildingManager Instance {
        get {
            if (instance == null) { 
                instance = (BuildingManager)FindObjectOfType(typeof(BuildingManager));;
            }
            return instance;
        }
    }

    public static float valueMultiplier = 1;
    public Grid grid;
    List<Apartment> portfolio = new List<Apartment>();
    bool hasTenantsUnion = false;

    void Awake() {
    	Events.instance.AddListener<BuyApartmentEvent>(OnBuyApartment);
    	Events.instance.AddListener<SellApartmentEvent>(OnSellApartment);
        Events.instance.AddListener<CompleteEvictionEvent>(OnCompleteEvictionEvent);
    }

    void Start() {
        Apartment a = grid.GetHighestValueApartment();
        a.SetAsHighestValue();
    }

    public float GetRentalIncome() {
    	return portfolio.Sum(a => { if (a.HasRent) { return a.Rent; } else { return 0; } });
    }

    public bool HasEvictionOrder() {
        return portfolio.Any(a => a.EvictionOrder);
    }

    void OnBuyApartment(BuyApartmentEvent e) {
    	portfolio.Add(e.Apartment);
    }

    void OnSellApartment(SellApartmentEvent e) {
    	portfolio.Remove(e.Apartment);
    }

    void OnCompleteEvictionEvent(CompleteEvictionEvent e) {
        if (!hasTenantsUnion) {
            Apartment a = grid.GetRandomApartment();
            a.MakeTenantsUnionHeadquarters();
        }
        hasTenantsUnion = true;
    }
}
