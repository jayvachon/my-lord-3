using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MB
{

	public GameObject panel;
	public Text text;

	void Start() {
		Show("this is a great test");
	}

    public void Show(string message) {
    	text.text = message;
    	panel.SetActive(true);
    	Co.WaitForSeconds(5, Hide);
    }

    public void Hide() {
    	panel.SetActive(false);
    }
}
