using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class SelectApartmentEvent : GameEvent {

		public readonly Apartment Apartment;

		public SelectApartmentEvent (Apartment apartment) {
			Apartment = apartment;
		}
	}
}