using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : Area {

	public override bool IsHighlightable() {
		if (App.zoom == Zoom.group) return true;
		return false;
	}

	public override bool Act() {
		if (isHighlightable) {
			SetZoom();
			return true;
		} else {
			//parentGroup?.CenterAndFocus(Zoom.group);
			return false;
		}
	}
	public override void SetZoom() {
		Center();
		switch (App.zoom) {
			case Zoom.large:
				parentGroup?.CenterAndFocus(Zoom.group);
				break;
			case Zoom.group:
				CenterAndFocus(Zoom.tile);
				break;
			case Zoom.tile:
				parentGroup?.CenterAndFocus(Zoom.group);
				break;
		}
	}
}