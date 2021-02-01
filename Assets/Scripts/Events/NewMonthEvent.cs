using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class NewMonthEvent : GameEvent {

		public readonly int Month;

		public NewMonthEvent (int month) {
			Month = month;
		}
	}
}