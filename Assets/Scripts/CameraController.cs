using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	float speed = 0.075f;

    void Update() {
    	if (Input.GetKey(KeyCode.DownArrow)) {
    		transform.position += new Vector3(-speed, 0, -speed);
    	}
    	if (Input.GetKey(KeyCode.UpArrow)) {
    		transform.position += new Vector3(speed, 0, speed);
    	}
    	if (Input.GetKey(KeyCode.LeftArrow)) {
    		transform.position += new Vector3(-speed, 0, speed);
    	}
    	if (Input.GetKey(KeyCode.RightArrow)) {
    		transform.position += new Vector3(speed, 0, -speed);
    	}
    }
}
