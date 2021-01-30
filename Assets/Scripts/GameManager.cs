using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	float valueTime = 0;

    void Update() {

    	valueTime += Time.deltaTime;
    	if (valueTime >= 30) {
    		BuildingManager.valueMultiplier += 0.1f;
    		Debug.Log(BuildingManager.valueMultiplier);
    		valueTime = 0;
    	}
    }
}
