using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class SellApartmentEvent : ApartmentEvent {

		public SellApartmentEvent (Apartment apartment) : base(apartment) {}
	}
}