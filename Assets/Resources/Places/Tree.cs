using System.Collections;
using UnityEngine;

public class Tree : Place {

	Branch branch;


	protected override void Start() {
		base.Start();
		if (item) {
			branch = item.GetComponent<Branch>();
			//item.Clickable(false);
			//branch.GetComponent<Rigidbody>().isKinematic = true;
		}
		isHighlightable = item != null;
	}

	public override bool IsInteractable() {
		return base.IsInteractable() && branch != null && !branch.GetComponent<Loot>().isClckable && App.zoom == Zoom.tile;
	}

	public override bool Act() {
		if (base.Act()) {
			((InventoryEntry)App.inventoryUI.selectedEntry.entry).Clear();
			Shake();
			return true;
		}
		return false;
	}

	void Shake() {
		if (branch != null) {
			//branch.isKinematic = false;
			//branch.AddForceAtPosition(impulse, Vector3.zero, ForceMode.Impulse);
			//branch.GetComponent<Loot>().Clickable(true);// .isHighlightable = true;
			//isHighlightable = false;
			branch.Cut();
			StartCoroutine(IShake());
		}

		IEnumerator IShake() {
			Vector3 kVector = new Vector3();
			transform.localScale = new Vector3(1, 1, 1);
			Vector3 start = transform.localScale;
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
					delta = -Mathf.Sin(a) / 15f;
					if (axis == Vector3.up) {
						kVector.y = delta;
					} else if (axis == Vector3.left) {
						kVector.x = delta;
					} else if (axis == Vector3.forward) {
						kVector.z = delta;
					}
					transform.localScale = start + kVector;
					App.camController.cameraTarget.transform.position = branch.transform.position;
				}
				if (resetOnExit) {
					transform.localScale = start;
					item = null;
					branch = null;
				}
			}

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