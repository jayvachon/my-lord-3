using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class BuyApartmentEvent : GameEvent {

		public readonly Apartment Apartment;

		public BuyApartmentEvent (Apartment apartment) {
			Apartment = apartment;
		}
	}
}