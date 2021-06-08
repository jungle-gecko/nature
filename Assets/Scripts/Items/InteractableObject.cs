using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using System.Reflection;

/// <summary>
/// Base class for interactable object, inherit from this class and override InteractWith to handle what happen when
/// the player interact with the object.
/// </summary>
public abstract class InteractableObject : HighlightableObject {
	public string itemName;
	[TextArea(1, 10)]
	public string description;
	public Vector3 center => GetComponent<Collider>().bounds.center;
	public Transform focusPoint;
	Vector3 focusPosition => focusPoint ? focusPoint.transform.position : center;
	public Tile parentTile { get; private set; }
	public TilesGroup parentGroup { get; private set; }
	public Rigidbody rb { get; private set; }


	public void Clickable(bool on) {
		isHighlightable = on;
		gameObject.GetComponent<Collider>().enabled = on;
		//var rb = GetComponent<Rigidbody>();
		//if (rb)
		//	rb.isKinematic = !on;
	}

	public virtual bool IsInteractable() {                  // l'objet est-il actif pour l'intéraction ?
		return IsHighlightable(); // && isInPlayerCollider && isClosest;
	}

	protected override void Start() {
		base.Start();
		if (GetComponentInChildren<Collider>() == null) {
			gameObject.AddComponent<SphereCollider>();
		}
		parentTile = GetComponentInParent<Tile>();
		parentGroup = GetComponentInParent<TilesGroup>();

		rb = GetComponent<Rigidbody>();
		if (rb)
			rb.isKinematic = true;

		//if (focusPoint) {
		//	focusPosition = focusPoint.position;
		//} else {
		//	focusPosition = center;
		//	focusPosition.y = GetComponent<Collider>().bounds.max.y;
		//}
	}

	protected virtual void Update() {
		// bouton d'action
		if (Input.GetButtonDown("Fire1") && !App.uiManager.isClicOnUI && isMouseOver && IsInteractable()) {
			Act();
		}
	}

	public abstract bool Act();

	public void Follow() {
		if (rb)
			StartCoroutine(IFollow());
		IEnumerator IFollow() {
			yield return null;
			while (rb.velocity.magnitude > Vector3.kEpsilon) {
				App.camController.cameraTarget.transform.position = center;
				yield return null;
			}
			rb.isKinematic = true;
		}
	}

	public virtual void Center() {
		App.camController.Center(focusPosition);
	}
	//public virtual void Center() {
	//	StartCoroutine(ICenter());
	//	IEnumerator ICenter() {
	//		float duration = .2f;
	//		float t = 0;
	//		float k = 0;
	//		Vector3 start = App.camController.cameraTarget.position;
	//		Vector3 end = focusPosition;

	//		while (t < duration) {
	//			t += Time.deltaTime;
	//			k = Mathf.Min(t / duration, 1f);
	//			App.camController.cameraTarget.position = Vector3.Lerp(start, end, k);
	//			yield return null;
	//		}
	//	}
	//}


	public void CenterAndFocus(Zoom z) {
		Center();
		App.camController.Focus(z);
	}

	//public override bool Highlight(bool on) {
	//	return base.Highlight(on);
	//}

	//public virtual SSavable Serialize() {
	//	return new SSavable() {
	//		version = App.saveVersion,
	//		guid = guid!=null ? ((Guid)guid).ToByteArray() : null,
	//		scene = gameObject.scene.name,
	//		position = transform.position.ToArray(),                 // position
	//		rotation = transform.rotation.ToArray(),                 // rotation
	//	};
	//}

	///// <summary>
	///// Restaurer les valeurs précédement sérialisées
	///// </summary>
	///// <param name="serialized">les valeurs sérialisées</param>
	//public virtual void Deserialize(object serialized) {
	//	if (serialized is SSavable) {
	//		SSavable s = serialized as SSavable;
	//		transform.position = s.position.ToVector();                     // position
	//		transform.rotation = s.rotation.ToQuaternion();                 // rotation
	//	}
	//}

}

/// <summary>
/// Classe pour la sauvegarde
/// 
/// Pour Loot : l'id de la 'target' sur laquelle il est posé
/// </summary>
[System.Serializable]
public class SItem : SSavable {
}