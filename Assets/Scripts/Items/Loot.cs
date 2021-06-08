using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Describes an InteractableObject that can be picked up and grants a specific item when interacted with.
///
/// It will also play a small animation (object going in an arc from spawn point to a random point around) when the
/// object is actually "spawned", and the object becomes interactable only when that animation is finished.
///
/// Finally it will notify the LootUI that a new loot is available in the world so the UI displays the name.
/// </summary>
public class Loot : InteractableObject {
	public List<Place> availableTargets;
	public Sprite itemSprite;
	public bool usable = false;

	public Entry entry { get; set; } = null;                                        // L'entrée d'inventaire lorsque l'objet a été ramassé

	protected override void Start() {
		base.Start();
		foreach (Place p in availableTargets) {
			p.Clickable(false);
		}
	}


	public bool isClckable => GetComponent<Collider>().enabled && isHighlightable;

	public override bool IsInteractable() {
		return base.IsInteractable() && App.zoom == Zoom.tile;
	}

	public override bool Act() {
		if (IsInteractable()) {
			Take();
			return true;
		} else {
			if (App.zoom != Zoom.tile)
				App.camController.Focus(Zoom.tile);
		}
		return false;
	}

	public bool Equals(Loot other) {
		return name == other.name;
	}


	protected virtual void Take() {

		EventSystem.current.SetSelectedGameObject(null);        // éviter les actions multiples en 1 seul clic
																// on ramasse l'objet

		if (!App.inventory.isFull) {                                                                // si l'inventaire n'est pas plein
			App.inventory.AddItem(this);                                                            //		ajouter l'objet à l'inventaire

			//SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData() { Clip = SFXManager.PickupSound });      //		son
			//App.targetsManager.OnTake();                                                                                        //		extinction de toutes les cibles
			//if (target) {			//		mise à jour de la cible (s'il était sur une cible)
			//	target.parentGroup.CenterAndFocus(Zoom.group);
			//	target.item = null;
			//	target = null;
			//}
			parentGroup?.CenterAndFocus(Zoom.group);
			parentTile?.Clickable(false);
		}
	}

	public void TakeAnimation(EntryUI ui) {
		StartCoroutine(ITake());
		IEnumerator ITake() {
			float duration = .5f;
			float t = 0;
			float k = 0;
			Vector3 startScale = transform.localScale;
			Vector3 endScale = startScale * 4f;
			Vector3 startPos = transform.position;
			if (GetComponent<Rigidbody>())
				GetComponent<Rigidbody>().isKinematic = true;
			foreach (Place p in availableTargets) {
				p.Clickable(true);
			}
			while (t < duration) {
				t += Time.deltaTime;
				k = Mathf.Min(t / duration, 1f);
				transform.localScale = Vector3.Lerp(endScale, startScale, k);
				transform.position = Vector3.Lerp(startPos, App.camController.cam.ScreenToWorldPoint(ui.transform.position), k);
				yield return null;
			}
			gameObject.SetActive(false);
			App.inventoryUI.Show(true);
			if (App.inventoryUI.firstUse) {
				App.inventoryUI.OnFirstUse();
			}

		}
	}

	public void HighlightTargets(bool on) {
		foreach(Place t in availableTargets) {
			t.Highlight(on);
		}
	}
	/// <summary>
	/// Déposer un objet d'inventaire
	/// </summary>
	/// <param name="target">le lieu</param>
	/// <param name="entry">l'entrée d'inventaire </param>
	public virtual void Drop() {
		App.inventoryUI.selectedEntry?.Select(false);
		App.inventory.RemoveItem((InventoryEntry)entry);		// retirer l'objet déposé de l'inventaire
	}

	#region sauvegarde
	/// <summary>
	/// Ajouter la sérialisation des infos à sauvegarder pour cet objet à la sauvegarde générale 'sav'
	/// </summary>
	public override SSavable Serialize() {
		var result = new SLoot().Copy(base.Serialize());
		result.itemBase = name;
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

/// <summary>
/// Classe pour la sauvegarde
/// 
/// Pour Loot : l'id de la 'target' sur laquelle il est posé
/// </summary>
[System.Serializable]
public class SLoot : SSavable {
	public string itemBase;
}