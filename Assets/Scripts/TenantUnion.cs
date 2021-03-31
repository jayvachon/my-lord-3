using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class TenantUnion : MB {

	static TenantUnion instance = null;
	static public TenantUnion Instance {
		get {
			if (instance == null) {
				instance = Object.FindObjectOfType (typeof (TenantUnion)) as TenantUnion;
				if (instance == null) {
					GameObject go = new GameObject ("TenantUnion");
					DontDestroyOnLoad (go);
					instance = go.AddComponent<TenantUnion>();
				}
			}
			return instance;
		}
	}

	int resentment = 0;

	public float ChanceOfEvictionRefusal {
		get { 
			return ((float)resentment / 10f);
		}
	}

    protected override void AddListeners() {
    	Events.instance.AddListener<CompleteEvictionEvent>(OnCompleteEvictionEvent);
    }

    void OnCompleteEvictionEvent(CompleteEvictionEvent e) {
    	resentment ++;
    	Debug.Log("resentment: " + resentment);
    }
}
