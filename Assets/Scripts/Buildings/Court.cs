using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Court : Building
{
	string[] laws = {
		"Just Cause"
	};

    void Update() {
    	if (Selected) {
    		if (Input.GetKeyDown(KeyCode.S)) {
    			if (BuildingManager.Instance.HasEvictionOrder()) {
    				Events.instance.Raise(new CourtCaseEvent());
    			}
    		}
    	}
    }
}
