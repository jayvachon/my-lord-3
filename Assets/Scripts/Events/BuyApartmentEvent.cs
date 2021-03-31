using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class BuyApartmentEvent : ApartmentEvent {
		public BuyApartmentEvent (Apartment apartment) : base(apartment) {}
	}
}