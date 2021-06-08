using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static App;
public class InventoryUI : UIBase
{
	public ItemEntryUI selectedEntry { get; private set; }
	public ItemEntryUI[] entries { get; private set; }
	public int capacity { get; private set; }
	public bool firstUse { get; set; } = true;
	[TextArea(1, 10)]
	public string firstUseText;

	public Combination[] combinations;
	
	EntryUI hoveredItem;

	private void Awake() {
		inventoryUI = this;
	}

	void Start() {
		entries = GetComponentsInChildren<ItemEntryUI>(true);
		capacity = entries.Length;
		inventory = new InventoryManager(capacity);
		panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height / 8f);
		Show(false);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.I)) {
			Toggle();
		}
	}

	public void Select(ItemEntryUI entry) {
		selectedEntry = entry;
	}

	public void OnFirstUse() {
		firstUse = false;
	}

	/// <summary>
	/// utiliser un objet (ex: boire une potion...)
	/// (inutilisé pour l'instant)
	/// </summary>
	/// <param name="usedItem"></param>
	public void ObjectDoubleClicked(Entry usedItem) {
		inventory.UseItem(usedItem);
		ObjectHoverExited(hoveredItem);
	}

	/// <summary>
	/// Survol par le pointeur de souris
	/// TODO: pour l'instant sans effet... à conserver ???
	/// </summary>
	/// <param name="hovered">L'entrée sous la souris</param>
	public void ObjectHoveredEnter(EntryUI hovered) {       // début de survol
		hoveredItem = hovered;
		if (hovered != selectedEntry)
			hoveredItem.label.enabled = true;
	}
	public void ObjectHoverExited(EntryUI exited) {         // fin de survol
		if (hoveredItem == exited) {
			if (exited!=selectedEntry)
				hoveredItem.label.enabled = false;
			hoveredItem = null;
		}
	}

	public override void Toggle() {
		panel.SetActive(!panel.activeInHierarchy);
		selectedEntry?.Select(false);
	}
}
