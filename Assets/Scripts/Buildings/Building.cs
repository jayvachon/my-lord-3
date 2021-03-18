using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class Building : Clickable
{
	float propertyValue = 0;
    public virtual float PropertyValue {
		get { return propertyValue; }
	}

	public float PropertyNeighborModifier {
		get; set;
	}

	public bool Selected { get; private set; }

	#region Clickable
    public override void ClickThis() {
        Toggle();
    }

    public override void ClickOther() {
        if (Selected) {
            Deselect();
        }
    }

    void Toggle() {
        if (Selected) {
            Deselect();
        } else {
            Select();
        }
    }

    void Select() {

        // To avoid race condition between de/selecting, always select last
        StartCoroutine(CoSelect());
    }
    IEnumerator CoSelect() {
        yield return new WaitForEndOfFrame();
        Events.instance.Raise(new SelectBuildingEvent(this));
        Selected = true;
        OnSelect();
    }

    void Deselect() {
        Events.instance.Raise(new DeselectBuildingEvent(this));
        Selected = false;
        OnDeselect();
    }
    #endregion

    protected virtual void OnSelect() {}
    protected virtual void OnDeselect() {}
}
