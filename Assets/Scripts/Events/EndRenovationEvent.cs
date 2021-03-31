using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class EndRenovationEvent : ApartmentEvent {

		public EndRenovationEvent (Apartment apartment) : base(apartment) {}
	}
}