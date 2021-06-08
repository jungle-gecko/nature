using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Loot
{

	public FishingRod rod;

	protected override void Take() {
		base.Take();
		rod.gameObject.SetActive(false);
	}
}
