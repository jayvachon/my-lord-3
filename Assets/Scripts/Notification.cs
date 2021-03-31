using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystem;

public class Notification : MB
{

	public GameObject panel;
	public Text text;
	public Color info;
	public Color attention;

	void Start() {
		Hide();
		Events.instance.AddListener<BuyApartmentEvent>(OnBuyApartmentEvent);
		Events.instance.AddListener<SellApartmentEvent>(OnSellApartmentEvent);
		Events.instance.AddListener<LeaseApartmentEvent>(OnLeaseApartmentEvent);
		Events.instance.AddListener<StartRenovationEvent>(OnStartRenovationEvent);
		Events.instance.AddListener<EndRenovationEvent>(OnEndRenovationEvent);

		Events.instance.AddListener<CompleteEvictionEvent>(OnCompleteEvictionEvent);
		Events.instance.AddListener<RefuseRentIncreaseEvent>(OnRefuseRentIncreaseEvent);
		Events.instance.AddListener<RefuseEvictionEvent>(OnRefuseEvictionEvent);

		Events.instance.AddListener<CourtCaseEvent>(OnCourtCaseEvent);
	}

    public void Show(string message, Color color) {
    	text.text = message;
    	panel.SetActive(true);
    	panel.GetComponent<Image>().color = color;
    	Co.WaitForSeconds(3, Hide);
    }

    public void Hide() {
    	panel.SetActive(false);
    }

    void OnBuyApartmentEvent(BuyApartmentEvent e) {
    	Show("Bought apartment for $" + e.Apartment.PropertyValue.ToDisplay(), info);
    }

    void OnSellApartmentEvent(SellApartmentEvent e) {
    	Show("Sold apartment for $" + e.Apartment.PropertyValue.ToDisplay(), info);
    }

    void OnLeaseApartmentEvent(LeaseApartmentEvent e) {
    	Show("Apartment leased!", info);
    }

    void OnStartRenovationEvent(StartRenovationEvent e) {
    	Show("6-month renovation started.", info);
    }

    void OnEndRenovationEvent(EndRenovationEvent e) {
    	Show("Renovation completed!", info);
    }

    void OnCompleteEvictionEvent(CompleteEvictionEvent e) {
    	Show("Tenants evicted!", info);
    }

    void OnRefuseRentIncreaseEvent(RefuseRentIncreaseEvent e) {
    	Show("Tenants refuse to pay rent increase", attention);
    }

    void OnRefuseEvictionEvent(RefuseEvictionEvent e) {
    	Show("Tenants refuse to leave!", attention);
    }

    void OnCourtCaseEvent(CourtCaseEvent e) {
    	Show("Case settled for $", info);
    }
}