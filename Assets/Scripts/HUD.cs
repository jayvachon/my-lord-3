using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystem;

public class HUD : MBUI
{
    public BuildingManager buildingManager;
	public Text purseText;
	public Text buildingInfoText;
    public Text clockText;
    public Text incomeText;

    public GameClock gameClock;

    Apartment selectedApartment = null;

    void Update() {
        
    	purseText.GetComponent<Text>().text = "$" + Purse.wealth.ToDisplay();
        clockText.GetComponent<Text>().text = "Month " + gameClock.Month.ToString();
        incomeText.GetComponent<Text>().text = "Rental income: $" + buildingManager.GetRentalIncome();

        if (selectedApartment != null) {
            string buildingInfo = "$" + selectedApartment.PropertyValue.ToDisplay();
            if (selectedApartment.Owned) {
                if (selectedApartment.HasTenants) {
                    buildingInfo += "\n<E> Evict tenants" +
                        "\n<S> Sell";
                    if (selectedApartment.NeedsRepair) {
                        buildingInfo += "\n<F> Fix";
                    }
                    if (selectedApartment.CanRaiseRent) {
                        buildingInfo += "\n<U> Raise Rent";
                    }
                } else {
                    if (!selectedApartment.Renovating) {
                        buildingInfo += "\n<R> Renovate for $" + selectedApartment.RenovationCost.ToDisplay() +
                            "\n<L> Lease" +
                            "\n<S> Sell";
                    }
                }
            } else {
                buildingInfo += "\n<B> Buy";
            }
            buildingInfoText.GetComponent<Text>().text = buildingInfo;
        }
    }

    protected override void AddListeners() {
    	Events.instance.AddListener<SelectApartmentEvent>(OnSelectApartmentEvent);
    	Events.instance.AddListener<DeselectApartmentEvent>(OnDeselectApartmentEvent);
    }

    void OnSelectApartmentEvent(SelectApartmentEvent e) {
        selectedApartment = e.Apartment;
    }

    void OnDeselectApartmentEvent(DeselectApartmentEvent e) {
        selectedApartment = null;
    	buildingInfoText.GetComponent<Text>().text = "";
    }
}
