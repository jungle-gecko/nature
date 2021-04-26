using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteractions : MonoBehaviour {
	public GameObject mainCamera;

	public static TilesController selectedTile;

	void Start() {

	}

	void Update() {
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				SelectObjectByMousePos();
			}
		}

		if (Input.GetMouseButtonDown(0)) {
			SelectObjectByMousePos();
		}
	}

	private void SelectObjectByMousePos() {
		// Construct a ray from the current touch coordinates
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//// Create a particle if hit
		//if (Physics.Raycast(ray, out hit))
		//{
		//    GameObject hitObject = hit.collider.gameObject;
		//    if (hitObject.GetComponent<TilesController>() != null)
		//    {
		//        if (selectedTile == null)
		//        {
		//            selectedTile = hitObject;
		//            mainCamera.GetComponent<CameraController>().Focus();
		//        }
		//        else if (selectedTile.GetInstanceID() != hitObject.GetInstanceID())
		//        {
		//            selectedTile = null;
		//            mainCamera.GetComponent<CameraController>().Unfocus();
		//        }
		//    }
		//}
	}
}