using System.Collections;
using UnityEngine;

public class Tent : Place {

	public Vector3 impulse;
	Rigidbody rod;



	protected override void Start() {
		base.Start();
		if (item) {
			rod = item.GetComponent<Rigidbody>();
			item.Clickable(false);
		}
		isHighlightable = item != null;
	}

	public override bool IsInteractable() {
		return rod != null && !rod.GetComponent<Loot>().isClckable && App.zoom == Zoom.tile;
	}

	public override bool Act() {
		if (base.Act()) {
			Shake();
			return true;
		}
		return false;
	}

	void Shake() {
		Vector3 kVector = new Vector3();
		transform.localScale = new Vector3(1, 1, 1);
		Vector3 start = transform.localScale;
		item.Clickable(true);
		StartCoroutine(IShake());
		StartCoroutine(ICenter());

		IEnumerator IShake() {
			if (rod != null) {
				rod.isKinematic = false;
				rod.AddForceAtPosition(impulse, Vector3.zero, ForceMode.Impulse);
				isHighlightable = false;
			}
			StartCoroutine(IAnimation(Vector3.up));
			yield return new WaitForSeconds(.05f);
			StartCoroutine(IAnimation(Vector3.left));
			yield return new WaitForSeconds(.05f);
			StartCoroutine(IAnimation(Vector3.forward, true));

			IEnumerator IAnimation(Vector3 axis, bool resetOnExit = false) {
				float delta;
				float duration = .5f;
				float t = 0;
				float a;
				while (t < duration) {
					yield return null;
					t += Time.deltaTime;
					a = Mathf.Lerp(0, Mathf.PI * 4f, t / duration);
					delta = -Mathf.Sin(a) / 5f;
					if (axis == Vector3.up) {
						kVector.y = delta;
					} else if (axis == Vector3.left) {
						kVector.x = delta;
					} else if (axis == Vector3.forward) {
						kVector.z = delta;
					}
					transform.localScale = start + kVector;
				}
				if (resetOnExit) {
					transform.localScale = start;
					item.Follow();
				}
			}

		}
		IEnumerator ICenter() {
			yield return new WaitForSeconds(1.5f);
			item.Center();
			item = null;
		}

	}

	#region sauvegarde
	/// <summary>
	/// Ajouter la sérialisation des infos à sauvegarder pour cet objet à la sauvegarde générale 'sav'
	/// </summary>
	public override SSavable Serialize() {
		var result = new SItem().Copy(base.Serialize());
		return result;
	}

	/// <summary>
	/// Restaurer les valeurs précédement  sérialisées 
	/// </summary>
	/// <param name="serialized"></param>
	public override void Deserialize(object serialized) {
		base.Deserialize(serialized);
		SLoot sLoot = (SLoot)serialized;
		//if (new System.Guid(sLoot.target) != System.Guid.Empty)
		//	target = Game.Find<Target>(sLoot.target);
	}

	#endregion
}