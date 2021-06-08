using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Place : Area {
	public Loot item;
	public Transform dropPoint;

	protected override void Start() {
		base.Start();
		if (!dropPoint) {
			dropPoint = new GameObject("DropPoint").transform;
			dropPoint.parent = transform;
			dropPoint.position = center;
		}
	}

	public override bool Act() {
		if (IsInteractable()) {
			EventSystem.current.SetSelectedGameObject(null);        // éviter les actions multiples en 1 seul clic

			if (item) {                                             // si l'endroit contient un objet => activer l'objet pour pouvoir le prendre
				item.isHighlightable = true;
			} else {												// si l'objet ne contient pas d'objet => y déposer l'objet d'inventaire sélectionné 
				item = App.inventoryUI.selectedEntry.loot;
				item.Drop();
				item.transform.SetParent(dropPoint);
				item.transform.localPosition = Vector3.zero;
				item.transform.localRotation = Quaternion.Euler(0, 0, 0);
				item.gameObject.SetActive(true);
			}
			Clickable(false);
			return true;
		}
		SetZoom();
		return false;
	}

	public override bool IsHighlightable() {
		foreach (InventoryEntry entry in App.inventory.entries) {
			if (CompatibleWith(entry.item))
				return base.IsHighlightable();// && item == null;
		}
		return false;
	}

	public override bool IsInteractable() {
		return IsHighlightable() && CompatibleWith(App.inventoryUI.selectedEntry?.loot);
	}

	bool CompatibleWith(Loot testItem) {
		return testItem != null && testItem.availableTargets.Contains(this);
	}

	public override void SetZoom() {
		Center();
		switch (App.zoom) {
			case Zoom.large:
				parentGroup?.CenterAndFocus(Zoom.group);
				//App.camController.Focus(Zoom.group);
				break;
			case Zoom.group:
				CenterAndFocus(Zoom.tile);
				//App.camController.Focus(Zoom.tile);
				break;
			case Zoom.tile:
				parentGroup?.CenterAndFocus(Zoom.group);
				//App.camController.Focus(Zoom.group);
				break;
		}
	}
}
