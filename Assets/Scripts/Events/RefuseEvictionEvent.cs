using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class RefuseEvictionEvent : ApartmentEvent {

		public RefuseEvictionEvent (Apartment apartment) : base(apartment) {}
	}
}