using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class SelectBuildingEvent : GameEvent {

		public readonly Building Building;

		public SelectBuildingEvent (Building building) {
			Building = building;
			if (Building is Apartment) {
				Events.instance.Raise(new SelectApartmentEvent(Building as Apartment));
			}
		}
	}
}