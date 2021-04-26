using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour {
	public Vector3 center { get; private set; }
	private Vector3 startPosition;
	private Transform cameraTarget;
	private CameraController cam;

	public void Start() {
		cam = Camera.main.GetComponent<CameraController>();
		cameraTarget = cam.transform.parent;
		startPosition = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
		center = GetComponent<Collider>().bounds.center;
		Appear();
	}

	public void Appear() {
		StartCoroutine(SlideDown());
	}

	private IEnumerator SlideDown() {
		var time = 0f;
		var duration = .2f;
		Vector3 pos = transform.position;
		while (time < duration) {
			time += 2 * Time.deltaTime;
			transform.position = Vector3.Lerp(pos, startPosition, time / duration);
			yield return null;
		}
		transform.position = startPosition;
	}

	private void OnMouseDown() {
		Center();
		if (UserInteractions.selectedTile == this) {
			cam.Unfocus();
			UserInteractions.selectedTile = null;
		} else {
			cam.Focus();
			UserInteractions.selectedTile = this;
		}
	}

	void Center() {
		StartCoroutine(ICenter());
		IEnumerator ICenter() {
			float duration = .2f;
			float t = 0;
			float k = 0;
			Vector3 start = cameraTarget.position;
			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				cameraTarget.position = Vector3.Lerp(start, center, k);
				yield return null;
			}
		}
	}
}
