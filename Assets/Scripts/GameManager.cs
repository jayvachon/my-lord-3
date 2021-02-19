using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class GameManager : MonoBehaviour
{
	void Awake() {
		Events.instance.AddListener<NewMonthEvent>(OnNewMonthEvent);
	}

	void OnNewMonthEvent(NewMonthEvent e) {
		if (e.Month % 12 == 0) {
			BuildingManager.valueMultiplier += 0.1f;
    		Debug.Log(BuildingManager.valueMultiplier);
		}
	}
}
