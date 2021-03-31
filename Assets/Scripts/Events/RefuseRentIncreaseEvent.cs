using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class RefuseRentIncreaseEvent : ApartmentEvent {

		public RefuseRentIncreaseEvent (Apartment apartment) : base(apartment) {}
	}
}