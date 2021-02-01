using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class GameClock : MB
{
    float monthTimer = 0;

    public int Month {
    	get; private set;
    }

    void Awake() {
    	Month = 1;
    }

    void Update() {
    	monthTimer += Time.deltaTime;
    	if (monthTimer >= 5) {
    		Month ++;
    		Events.instance.Raise(new NewMonthEvent(Month));
    		monthTimer = 0;
    	}
    }
}
