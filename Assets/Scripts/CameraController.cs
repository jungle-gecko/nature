using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private Transform cameraTarget;
	private Camera cameraComponent;
	public int speed = 10;
	public int defaultZoom = 50;
	public int focusZoom = 20;

	void Start() {
		cameraTarget = transform.parent;
		cameraComponent = GetComponent<Camera>();
	}

	public void Focus() {
		StartCoroutine(IFocus(focusZoom));
	}

	public void Unfocus() {
		StartCoroutine(IFocus(defaultZoom));
	}
	IEnumerator IFocus(float zoom) {
		float duration = .2f;
		float t = 0;
		float k = 0;
		float start = cameraComponent.orthographicSize;
		while (t < duration) {
			t += Time.deltaTime;
			k = Mathf.Min(t / duration, 1f);
			cameraComponent.orthographicSize = Mathf.Lerp(start, zoom, k);
			yield return null;
		}

	}


}
