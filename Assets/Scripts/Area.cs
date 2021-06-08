using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : InteractableObject {
	public override bool Act() {
		return false;
	}

	public virtual void SetZoom() {
		App.camController.Focus(Zoom.group);
	}
}

public class Areas {
	public TilesGroup group;
	public Tile tile;
	public Place place;

	public TilesGroup selectedGroup;
	public Tile selectedTile;
	public Place selectedPlace;

	public void Clear() {
		group = null;
		tile = null;
		place = null;
	}

	public void Unselect() {
		selectedGroup = null;
		selectedPlace = null;
		selectedTile = null;
	}

	public bool isEmpty => group == null && tile == null && place == null;

	public bool Raycast(RaycastHit[] hits, int count) {
		Clear();
		
		for (int i = 0; i < count; i++) {
			if (hits[i].collider.GetComponent<Place>())
				place = App.GetClosest<Place>(place, hits[i].collider.GetComponent<Place>());
			if (hits[i].collider.GetComponent<Tile>())
				tile = App.GetClosest<Tile>(place, hits[i].collider.GetComponent<Tile>());
			if (hits[i].collider.GetComponent<TilesGroup>())
				group = App.GetClosest<TilesGroup>(place, hits[i].collider.GetComponent<TilesGroup>());
		}
		return !isEmpty;
	}

	public void Select(Place p, bool on = true) {
		if (on) selectedPlace = place;
		else selectedPlace = null;
	}
	public void Select(Tile p, bool on = true) {
		if (on) selectedTile = tile;
		else selectedTile = null;
	}
	public void Select(TilesGroup p, bool on = true) {
		if (on) { 
			selectedGroup = group;
			selectedGroup.CenterAndFocus(Zoom.group);
		} 
		else selectedGroup = null;
	}
}
