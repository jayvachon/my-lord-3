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

    void Update() {
    	purseText.GetComponent<Text>().text = "$" + Purse.wealth.ToDisplay();
        clockText.GetComponent<Text>().text = "Month " + gameClock.Month.ToString();
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
