using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
    public static GameManager Instance {
        get {
        	if (instance == null) { 
        		instance = (GameManager)FindObjectOfType(typeof(GameManager));;
        	}
        	return instance;
        }
    }

    public bool GlobalRentStrike {
    	get { return false; }
    }

	void Awake() {
		Events.instance.AddListener<NewMonthEvent>(OnNewMonthEvent);
	}

	void OnNewMonthEvent(NewMonthEvent e) {
		if (e.Month % 12 == 0) {
			BuildingManager.valueMultiplier += 0.1f;
    		Debug.Log(BuildingManager.valueMultiplier);
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			Debug.Log("Game Manager added $100,000 to purse");
			Purse.wealth += 100000;
		}
	}
}
