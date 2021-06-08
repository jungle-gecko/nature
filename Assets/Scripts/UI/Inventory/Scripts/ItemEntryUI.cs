using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using static App;


public class ItemEntryUI : EntryUI
{
	public GameObject content;
	public Image iconeImage;
	public Image plus;
	public TMP_Text count;

	public Loot loot { get; set; }

	public bool isFree => entry == null || (entry as InventoryEntry).count <= 0;

	Color selectedColor = Color.yellow;
	Color unselectedColor = new Color(1f, 1f, 1f, .5f);

	private void Start() {
		Show(!isFree);
	}

	public override void Init(Entry entry) {
		InventoryEntry iEntry = entry as InventoryEntry;
		iEntry.ui = this;
		this.entry = iEntry;
		loot = iEntry.item;
		loot.entry = iEntry;
		iconeImage.sprite = loot.itemSprite;
		count.text = "";
		label.text = loot.itemName;
		label.color = unselectedColor;
		Show(true);
	}

	/// <summary>
	/// mise à jour
	/// </summary>
	public override void UpdateEntry() {
		bool isEnabled = entry != null && (entry as InventoryEntry)?.count > 0;
		InventoryEntry iEntry = entry as InventoryEntry;

		if (isEnabled) {
			iconeImage.sprite = iEntry?.item.itemSprite;

			if (iEntry?.count > 1) {
				count.gameObject.SetActive(true);
				count.text = iEntry?.count.ToString();
			} else {
				count.gameObject.SetActive(false);
			}
			Show(true);
		} else {
			inventoryUI.selectedEntry?.Select(false);
			inventory.entries.Remove(iEntry);
			Show(false);
		}
	}

	public override void Toggle() {
		TestCombine();
		foreach (ItemEntryUI entry in inventoryUI.entries) {                    // désélectionner toutes les autres entrées de l'inventaire
			if (entry != this && entry.selected)
				entry.Select(false);
		}
		base.Toggle();                                                          // sélectionner/déselectionner cette entrée
		EventSystem.current.SetSelectedGameObject(null);
	}

	void Show(bool on) {
		content.SetActive(on);
	}

	public override void Select(bool on) {
		if (on) {
			label.color = selectedColor;
			iconeImage.transform.localPosition = new Vector2(0, 20);
			iconeImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
			inventoryUI.Select(this);
		} else {
			label.color = unselectedColor;
			iconeImage.transform.localPosition = new Vector2(0, 0);
			iconeImage.transform.localScale = new Vector3(.9f, .9f, .9f);
			inventoryUI.Select(null);
		}
		loot.HighlightTargets(on);
		label.enabled = on;
		selected = on;
	}

	bool TestCombine() {
		if (!App.inventoryUI.selectedEntry) 
			return false;

		Loot l1, l2;
		foreach (Combination c in App.inventoryUI.combinations) {
			l1 = loot;
			l2 = App.inventoryUI.selectedEntry.loot;
			if ((l1 == c.item1 && l2 == c.item2) || (l2 == c.item1 && l1 == c.item2))
				return c.Combine();
		}
		return false;
	}
}
