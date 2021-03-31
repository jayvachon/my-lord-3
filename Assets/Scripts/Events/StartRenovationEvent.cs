using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class StartRenovationEvent : ApartmentEvent {

		public StartRenovationEvent (Apartment apartment) : base(apartment) {}
	}
}