using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Place {

	public TilesGroup nextGroup;

	public override bool Act() {
		if (base.Act()) {
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
			Vector3 startAngle = transform.localRotation.eulerAngles;
			Vector3 endAngle = new Vector3(startAngle.x, startAngle.y - 160, startAngle.z);
			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				transform.localRotation = Quaternion.Euler(Vector3.Lerp(startAngle, endAngle, k));
				yield return null;
			}
			Clickable(false);
			nextGroup.isActive = true;

			CenterAndFocus(Zoom.group);
			item.gameObject.SetActive(false);

		}

	}
}
