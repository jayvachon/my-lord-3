using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class RentStrikeEvent : ApartmentEvent {

		public RentStrikeEvent (Apartment apartment) : base(apartment) {}
	}
}