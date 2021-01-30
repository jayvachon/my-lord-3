using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB : MonoBehaviour
{
    MeshRenderer _mesh = null;
	protected MeshRenderer mesh {
		get {
			if (_mesh == null) {
				_mesh = GetComponent<MeshRenderer>();
			}
			return _mesh;
		}
	}

	void OnEnable() {
		AddListeners();
	}
	
	protected virtual void AddListeners() {}
}
