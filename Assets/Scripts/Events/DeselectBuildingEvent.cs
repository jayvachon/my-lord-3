using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class DeselectBuildingEvent : GameEvent {

		public readonly Building Building;

		public DeselectBuildingEvent (Building building) {
			Building = building;
			if (Building is Apartment) {
				Events.instance.Raise(new DeselectApartmentEvent());
			}
		}
	}
}