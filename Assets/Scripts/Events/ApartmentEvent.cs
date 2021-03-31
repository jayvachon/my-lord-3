using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem {

	public abstract class ApartmentEvent : GameEvent {
	    
	    public Apartment Apartment { get; protected set; }

		public ApartmentEvent (Apartment apartment) {
			Apartment = apartment;
		}
	}
}