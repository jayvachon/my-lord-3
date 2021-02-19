using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhousedPerson : MB
{
	float count = 0;
    void Update() {
    	count += Time.deltaTime;
    	transform.position = new Vector3(Mathf.Sin(count * 0.1f) * 8f, 0.26f, -1f);
    }
}
