using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Court : Building
{
    public override void ClickThis() {
    	Events.instance.Raise(new CourtCaseEvent());
    }
}
