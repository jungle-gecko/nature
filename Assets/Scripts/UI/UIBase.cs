﻿using System;
using UnityEngine;


/// <summary>
/// Handle all the UI code related to the inventory (drag'n'drop of object, using objects, equipping object etc.)
/// </summary>
public abstract class UIBase : MonoBehaviour
{

	public GameObject panel;
	public Action callback;

	public virtual bool isOn => (panel==null && gameObject.activeInHierarchy) || panel.activeInHierarchy;


	public virtual void Toggle() {
		Show(!panel.activeInHierarchy);
	}

	public virtual void Show(bool on) {
		panel.SetActive(on);
		if (!on && callback != null) {
			callback.Invoke();
			callback = null;
		}
	}
}
