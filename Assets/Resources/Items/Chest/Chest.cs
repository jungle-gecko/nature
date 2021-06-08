using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Loot {

	public Transform couvercle;
	public Hole hole;

	public override bool Act() {
		if (IsInteractable()) {
			Open();
			return true;
		}
		return false;
	}

	void Open() {
		StartCoroutine(IOpen());
		IEnumerator IOpen() {
			float duration = .4f;
			float t = 0;
			float k;
			Vector3 startAngle = couvercle.localRotation.eulerAngles;
			Vector3 endAngle = new Vector3(startAngle.x - 60, startAngle.y, startAngle.z);
			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				couvercle.localRotation = Quaternion.Euler(Vector3.Lerp(startAngle, endAngle, k));
				yield return null;
			}
			Clickable(false);
			hole.Clickable(false);
			//CenterAndFocus(Zoom.group);
			//item.gameObject.SetActive(false);

		}

	}
}
