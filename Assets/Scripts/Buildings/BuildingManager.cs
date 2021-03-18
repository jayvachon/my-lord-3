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
    List<Apartment> portfolio = new List<Apartment>();

    void Awake() {
    	Events.instance.AddListener<BuyApartmentEvent>(OnBuyApartment);
    	Events.instance.AddListener<SellApartmentEvent>(OnSellApartment);
    }

    public float GetRentalIncome() {
    	return portfolio.Sum(a => a.Rent);
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
}
