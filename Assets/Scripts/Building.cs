using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Building : Clickable
{
	float propertyValue = 0;
    public virtual float PropertyValue {
		get { return propertyValue; }
	}

	public float PropertyNeighborModifier {
		get; set;
	}
}
