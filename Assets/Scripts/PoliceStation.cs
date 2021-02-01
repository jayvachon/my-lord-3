using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class PoliceStation : Clickable
{
    public override void ClickThis() {
     	Events.instance.Raise(new CallPoliceEvent());
    }
}
