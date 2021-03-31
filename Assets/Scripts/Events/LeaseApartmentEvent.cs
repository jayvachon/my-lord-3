using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class LeaseApartmentEvent : ApartmentEvent {

		public LeaseApartmentEvent (Apartment apartment) : base(apartment) {}
	}
}