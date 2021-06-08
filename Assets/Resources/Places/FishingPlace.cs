using System.Collections;
using UnityEngine;

public class FishingPlace : Place {

	FishingRod rod;

	public override bool Act() {
		if (base.Act()) {
			rod = (FishingRod)item;
			if (rod) {
				rod.transform.localEulerAngles = new Vector3(-170, 180, 180);
				rod.Fishing();
			}
			return true;
		}
		return false;
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