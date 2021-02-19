using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public abstract class SelectApartmentListener : MB
{
	protected Apartment SelectedApartment { get; private set; }

    protected override void AddListeners() {
    	Events.instance.AddListener<SelectApartmentEvent>(OnSelectApartmentEvent);
    	Events.instance.AddListener<DeselectApartmentEvent>(OnDeselectApartmentEvent);
    }

    protected void OnSelectApartmentEvent(SelectApartmentEvent e) {
    	SelectedApartment = e.Apartment;
    	OnSelect();
    }

    protected void OnDeselectApartmentEvent(DeselectApartmentEvent e) {
    	SelectedApartment = null;
    	OnDeselect();
    }

    protected virtual void OnSelect() {}
    protected virtual void OnDeselect() {}
}
