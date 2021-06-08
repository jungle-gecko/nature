using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesGroup : Area {

	public bool active = false;

	public Fence[] fences;
	public InteractableObject[] interactableObjects;

	public bool isActive {
		get { return active; }
		set {
			active = value;
			Highlight(value);
		}
	}

	private Vector3 startPosition;
	private List<HighlightableMaterial> materials;

	protected override void Awake() {
		base.Awake();

		highlightFactor = .1f;
		materials = new List<HighlightableMaterial>();
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			materials.Add(new HighlightableMaterial(renderer, highlightFactor));
		}
		//interactableObjects = GetComponentsInChildren<InteractableObject>();
	}

	protected override void Start() {
		base.Start();

		Highlight(isActive);
		//Appear();
	}

	public void Appear() {
		startPosition = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
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

	public override void SetZoom() {
		Center();
		switch (App.zoom) {
			case Zoom.large:
				App.camController.Focus(Zoom.group);
				break;
			//case Zoom.group:
			//	App.camController.Focus(Zoom.large);
			//	break;
			case Zoom.tile:
				CenterAndFocus(Zoom.group);
				break;
		}
	}

	public override bool Act() {
		SetZoom();
		return base.Act();
	}


	public override bool Highlight(bool on) {
		if (!enabled) return false;
		App.fences.ShowFences();
		foreach (InteractableObject obj in interactableObjects) {
			obj.Clickable(on);
		}
		Clickable(on);
		return HighlightMats(on);
	}

	public new bool HighlightMats(bool on) {
		if (!enabled) return false;
		foreach (HighlightableMaterial m in materials) {
			m.Highlight(!on);
		}
		return true;
	}

	public void Activate(bool on) {
		if (on) {

		} else {

		}
		isActive = on;
	}
}
