using System.Collections;
using UnityEngine;

public class Hole : Place {

	Shovel shovel;

	public override bool Act() {
		if (IsHighlightable()) {
			base.Act();
			shovel = (Shovel)item;
			item = null;
			if (shovel) {
				shovel.Dig();
			}
			Clickable(true);
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