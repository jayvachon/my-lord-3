using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystem;

public class HUD : MBUI
{
	public Text purseText;
	public Text buildingInfoText;
    public Text clockText;

    public GameClock gameClock;

    Building selectedBuilding = null;

    void Update() {
        
    	purseText.GetComponent<Text>().text = "$" + Purse.wealth.ToDisplay();
        clockText.GetComponent<Text>().text = "Month " + gameClock.Month.ToString();

        if (selectedBuilding != null) {
            string buildingInfo = "$" + selectedBuilding.Value.ToDisplay();
            if (selectedBuilding.Owned) {
                if (selectedBuilding.HasTenants) {
                    buildingInfo += "\n<E> Evict tenants" +
                        "\n<S> Sell";
                } else {
                    if (!selectedBuilding.Renovating) {
                        buildingInfo += "\n<R> Renovate for $" + selectedBuilding.RenovationCost.ToDisplay() +
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
    	Events.instance.AddListener<SelectBuildingEvent>(OnSelectBuildingEvent);
    	Events.instance.AddListener<DeselectBuildingEvent>(OnDeselectBuildingEvent);
    }

    void OnSelectBuildingEvent(SelectBuildingEvent e) {
        selectedBuilding = e.Building;
    }

    void OnDeselectBuildingEvent(DeselectBuildingEvent e) {
        selectedBuilding = null;
    	buildingInfoText.GetComponent<Text>().text = "";
    }
}
