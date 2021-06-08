using System;
using System.Collections.Generic;
using UnityEngine;

using static App;



/// <summary>
/// All object that can be highlighted (enemies, interactable object etc.) derive from this class, which takes care
/// of setting the material parameters for it when it gets highlighted.
/// If the object use another material, it will just ignore all the changes.
/// </summary>
//[RequireComponent(typeof(BoxCollider))]
public abstract class HighlightableObject : Savable {

	public bool isHighlightable = true;

	public bool isOn { get; set; } = false;               // flag allumé ?
	public bool isMouseOver { get; set; } = false;

	private List<HighlightableMaterial> materials;
	public float highlightFactor = 1.1f;

	protected override void Start() {
		base.Start();

		materials = new List<HighlightableMaterial>();
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			materials.Add(new HighlightableMaterial(renderer, highlightFactor));
		}

		Highlight(false);
	}

	private void OnEnable() {
		if (materials != null)
			SetColor(highlightFactor);
	}

	public virtual bool IsHighlightable() {
		return isHighlightable;
	}

	//public virtual void OnMouseEnter() {
	//	isMouseOver = true;
	//	if (IsHighlightable()) {
	//		isOn = true;
	//		Highlight(true);
	//	}
	//}

	//public virtual void OnMouseExit() {
	//	isMouseOver = false;
	//	if (isOn) {
	//		Highlight(false);
	//		isOn = false;
	//	}
	//}

	public virtual bool Highlight(bool on) {
		if (!enabled) return false;
		return HighlightMats(on);
	}

	public void SetColor(float highlightFactor) {
		if (materials.Count>0)
			foreach (HighlightableMaterial m in materials) {
				m.SetColor(highlightFactor);
			}
	}

	public bool HighlightMats(bool on) {
		if (!enabled) return false;

		foreach (HighlightableMaterial m in materials) {
			m.Highlight(on);
		}
		return true;
	}
}