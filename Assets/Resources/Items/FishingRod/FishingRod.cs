using System.Collections;
using UnityEngine;

public class FishingRod : Loot {

	public bool isFishing = false;
	public Transform hook;
	public Loot key;

	Vector3 down = new Vector3(-90, 0, 0);

	protected override void Start() {
		base.Start();
		key.gameObject.SetActive(false);
		Combination combination = GetComponent<Combination>();
		if (combination) {
			combination.result = this;
			gameObject.SetActive(false);
		}
	}

	public void Fishing() {
		isFishing = true;
		StartCoroutine(IFishing());
		IEnumerator IFishing() {
			float duration = .4f;
			float t = 0;
			float k;
			Vector3 startAngle = transform.localRotation.eulerAngles;
			Vector3 endAngle = new Vector3(startAngle.x - 70, startAngle.y, startAngle.z);
			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				transform.localRotation = Quaternion.Euler(Vector3.Lerp(startAngle, endAngle, k));
				yield return null;
			}
			yield return new WaitForSeconds(duration*2);
			key.gameObject.SetActive(true);
			t = 0;
			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				transform.localRotation = Quaternion.Euler(Vector3.Lerp(endAngle, startAngle, k));
				yield return null;
			}
			isFishing = false;
			Clickable(false);
			GetComponent<Collider>().enabled = false;
			CenterAndFocus(Zoom.tile);
		}
	}

	protected override void Update() {
		base.Update();
		if (isFishing) {
			float a = transform.localRotation.eulerAngles.x;
			hook.rotation = Quaternion.Euler(down);
		}
	}
}
