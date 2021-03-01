using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class SellApartmentEvent : GameEvent {

		public readonly Apartment Apartment;

		public SellApartmentEvent (Apartment apartment) {
			Apartment = apartment;
		}
	}
}