using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination : MonoBehaviour {
	public Loot item1;
	public Loot item2;
	public Loot result { get; set; }

	public bool Combine() {
		if (App.inventory.Contains(item1) && App.inventory.Contains(item2) && result != null) {
			App.inventory.RemoveItem(item1);
			App.inventory.RemoveItem(item2);
			result.gameObject.SetActive(true);
			foreach(Place p in result.availableTargets) {
				p.Clickable(true);
			}
			Zoom z = App.zoom;
			App.camController.currentZoom = Zoom.tile;
			result.Act();
			App.camController.currentZoom = z;
			return true;
		} else {
			return false;
		}
	}

	public bool Exists(Loot l1, Loot l2) {
		if ((l1 == item1 && l2 == item2) || (l2 == item1 && l1 == item2))
			return Combine();
		return false;
	}
}
