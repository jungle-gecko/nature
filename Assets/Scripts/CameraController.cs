using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform cameraTarget { get; private set; }
	public Vector3 takePosition { get; private set; }
	public Camera cam { get; private set; }
	public int speed = 10;
	public int zoomLarge = 30;
	public int zoomGroup = 15;
	public int zoomTile = 5;
	public Zoom currentZoom { get; set; }

	Vector3 camPos;

	private void Awake() {
		cameraTarget = transform.parent;
		cam = GetComponent<Camera>();
		App.camController = this;
		camPos = transform.localPosition;
		takePosition = (transform.position + cameraTarget.position) / 2f;

	}

	public bool ZoomIn() {
		if (currentZoom == Zoom.tile)
			return false;
		if (currentZoom == Zoom.group)
			Focus(Zoom.tile);
		else
			Focus(Zoom.group);
		return true;
	}


	public void Focus(Zoom z) {
		StartCoroutine(IFocus(z));
		IEnumerator IFocus(Zoom z) {
			int zoom = GetZoom(z);
			float duration = .2f;
			float t = 0;
			float start = cam.orthographicSize;
			while (t < duration) {
				t += Time.deltaTime;
				cam.orthographicSize = Mathf.Lerp(start, zoom, t / duration);
				transform.localPosition = camPos * zoom / zoomLarge;
				yield return null;
			}
			takePosition = (transform.position + cameraTarget.position) / 2f;
			currentZoom = z;
		}
	}


	public virtual void Center(Vector3 focusPosition) {
		StartCoroutine(ICenter());
		IEnumerator ICenter() {
			float duration = .2f;
			float t = 0;
			float k = 0;
			Vector3 start = cameraTarget.position;
			Vector3 end = focusPosition;

			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				cameraTarget.position = Vector3.Lerp(start, end, k);
				yield return null;
			}
		}
	}

	public void Rotate(float angle, Action callback = null) {
		StartCoroutine(IRotate(angle));
		IEnumerator IRotate(float angle) {
			float duration = .2f;
			float t = 0;
			Quaternion start = cameraTarget.transform.rotation;
			Quaternion end = start * Quaternion.AngleAxis(angle, Vector3.up);
			while (t < duration) {
				t += Time.deltaTime;
				cameraTarget.transform.rotation = Quaternion.Slerp(start, end, t / duration);
				yield return null;
			}
			cameraTarget.transform.rotation = end;
			if (callback != null)
				callback();
		}
	}

	int GetZoom(Zoom z) {
		switch (z) {
			case Zoom.tile:
				return zoomTile;
			case Zoom.group:
				return zoomGroup;
			default:
				return zoomLarge;
		}
	}
}

public enum Zoom { large, group, tile }
