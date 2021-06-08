using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : Loot {
	public Vector3 impulse;

	protected override void Start() {
		base.Start();
		Clickable(false);
	}

	public void Cut() {
		rb.isKinematic = false;
		StartCoroutine(ICut());
		Clickable(true);	// .isHighlightable = true;
		isHighlightable = true;
		IEnumerator ICut() {
			yield return new WaitForSeconds(.1f);
			rb.AddForceAtPosition(impulse, Vector3.zero, ForceMode.Impulse);
			Follow();
		}
	}
}
