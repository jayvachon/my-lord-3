using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class SelectBuildingEvent : GameEvent {

		public readonly Building Building;

		public SelectBuildingEvent (Building building) {
			Building = building;
		}
	}
}