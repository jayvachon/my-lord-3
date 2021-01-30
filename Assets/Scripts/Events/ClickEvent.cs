using UnityEngine;
using System.Collections;

namespace EventSystem {

	public class ClickEvent : GameEvent {

		public readonly Transform Transform;

		public ClickEvent (Transform transform) {
			Transform = transform;
		}
	}
}