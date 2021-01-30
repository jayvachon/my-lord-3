using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystem;

public class HUD : MBUI
{
	public Text purseText;
	public Text buildingInfoText;

    void Update() {
    	purseText.GetComponent<Text>().text = "$" + Purse.wealth.ToDisplay();
    }

    protected override void AddListeners() {
    	Events.instance.AddListener<SelectBuildingEvent>(OnSelectBuildingEvent);
    	Events.instance.AddListener<DeselectBuildingEvent>(OnDeselectBuildingEvent);
    }

    void OnSelectBuildingEvent(SelectBuildingEvent e) {
    	buildingInfoText.GetComponent<Text>().text = "$" + e.Building.Value.ToDisplay();
    }

    void OnDeselectBuildingEvent(DeselectBuildingEvent e) {
    	buildingInfoText.GetComponent<Text>().text = "";
    }
}
