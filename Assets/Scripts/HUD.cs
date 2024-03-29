﻿using System.Collections;
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
    bool courtSelected = false;

    void Update() {
        
    	purseText.GetComponent<Text>().text = "Wealth: $" + Purse.wealth.ToDisplay();
        clockText.GetComponent<Text>().text = "Month " + gameClock.Month.ToString();
        incomeText.GetComponent<Text>().text = "Rental income: $" + buildingManager.GetRentalIncome().ToDisplay();

        if (selectedApartment != null) {
            string buildingInfo = "Property Value: $" + selectedApartment.PropertyValue.ToDisplay();
            if (selectedApartment.Owned) {
                if (selectedApartment.HasTenants) {
                    buildingInfo += "\n<E> Evict tenants" +
                        "\n<S> Sell";
                    if (selectedApartment.NeedsRepair) {
                        buildingInfo += "\n<F> Fix ($" + selectedApartment.RepairCost + ")";
                        // buildingInfo += "\n<C> Ignore repair ($0)";
                    }
                    if (selectedApartment.CanRaiseRent) {
                        buildingInfo += "\n<U> Raise Rent";
                    }
                    if (!selectedApartment.Renovating && selectedApartment.CanRenovate) {
                        buildingInfo += "\nEvict tenants to renovate for $" + selectedApartment.RenovationCost.ToDisplay();
                    }
                } else {
                    if (!selectedApartment.Renovating) {
                        if (selectedApartment.CanRenovate) {
                            buildingInfo += "\n<R> Renovate for $" + selectedApartment.RenovationCost.ToDisplay();
                        }
                        buildingInfo +=
                            "\n<L> Lease" +
                            "\n<S> Sell";
                    }
                }
            } else {
                buildingInfo += "\n<B> Buy";
            }
            buildingInfoText.GetComponent<Text>().text = buildingInfo;
        }

        if (courtSelected) {
            string buildingInfo = "Laws:";
            if (buildingManager.HasEvictionOrder()) {
                buildingInfo += "\n<S> Settle evictions";
            }
            buildingInfoText.GetComponent<Text>().text = buildingInfo;
        }
    }

    protected override void AddListeners() {
    	Events.instance.AddListener<SelectApartmentEvent>(OnSelectApartmentEvent);
    	Events.instance.AddListener<DeselectApartmentEvent>(OnDeselectApartmentEvent);
        Events.instance.AddListener<SelectBuildingEvent>(OnSelectBuildingEvent);
        Events.instance.AddListener<DeselectBuildingEvent>(OnDeselectBuildingEvent);
    }

    void OnSelectApartmentEvent(SelectApartmentEvent e) {
        selectedApartment = e.Apartment;
    }

    void OnDeselectApartmentEvent(DeselectApartmentEvent e) {
        selectedApartment = null;
    	buildingInfoText.GetComponent<Text>().text = "";
    }

    void OnSelectBuildingEvent(SelectBuildingEvent e) {
        if (e.Building is Court) {
            courtSelected = true;
        }
    }

    void OnDeselectBuildingEvent(DeselectBuildingEvent e) {
        courtSelected = false;
        buildingInfoText.GetComponent<Text>().text = "";
    }
}
