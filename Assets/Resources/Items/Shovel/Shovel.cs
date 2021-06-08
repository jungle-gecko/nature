using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Loot
{
	public GameObject ground, groundWithHole, dirt, chest;

	protected override void Take() {
		base.Take();
		gameObject.SetActive(false);
	}

	public override bool IsInteractable() {
		return IsHighlightable(); 
	}

	public void Dig() {
		App.inventory.RemoveItem(this);
		ground.SetActive(false);
		groundWithHole.SetActive(true);
		dirt.SetActive(true);
		chest.SetActive(true);
		//Clickable(false);
		gameObject.SetActive(false);
	}
}
