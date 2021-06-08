using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fences : MonoBehaviour {
	Fence[] fences;

	void Awake() {
		fences = GetComponentsInChildren<Fence>();
		App.fences = this;
	}

	public void ShowFences() {
		foreach (Fence f in fences) {
			f.Show();
		}
	}

}
