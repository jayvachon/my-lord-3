using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public abstract class Clickable : MB, IClickable {

	protected override void AddListeners() {
		Events.instance.AddListener<ClickEvent>(OnClickEvent);
	}

	void OnMouseDown() {
		Events.instance.Raise(new ClickEvent(transform));
	}

	void OnClickEvent(ClickEvent e) {
		if (e.Transform == transform) {
			ClickThis();
		} else {
			ClickOther();
		}
	}

	public virtual void ClickThis() {}
	public virtual void ClickOther() {}
}
