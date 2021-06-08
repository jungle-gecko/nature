using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
	public TilesGroup group1;
	public TilesGroup group2;
	public bool isActive => group1 && group1.isActive ? true : group2 && group2.isActive;

	Renderer[] renderers;

	private void Awake() {
		renderers = GetComponentsInChildren<Renderer>();
	}

	public void Show() {
		foreach (Renderer r in renderers) {
			r.enabled = isActive;
			if(gameObject.GetComponent<Collider>())
				gameObject.GetComponent<Collider>().enabled = isActive;
		}
	}
}
