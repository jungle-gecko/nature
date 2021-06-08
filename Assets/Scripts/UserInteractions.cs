using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UserInteractions : MonoBehaviour {
	private CameraController camController;
	private Camera cam;


	public Loot clickedItem { get; set; }
	public Loot selectedItem { get; set; }
	public Areas clickedAreas { get; set; } = new Areas();
	public Transform target { get; private set; }

	private void Awake() {
		App.player = this;
		camController = Camera.main.GetComponent<CameraController>();
		onSwipeLeft.AddListener(OnSwipeLeft);
		onSwipeRight.AddListener(OnSwipeRight);
	}

	void Start() {
		cam = camController.cam;
	}

	void Update() {
		DetectGesture();
	}


	RaycastHit[] hits = new RaycastHit[16];
	private bool isClicOnObject() {
		Ray ray = cam.ScreenPointToRay(_fingerDown);

		clickedItem = null;
		clickedAreas.Clear();

		int count = Physics.RaycastNonAlloc(ray, hits);
		if (count == 0)
			return false;

		for (int i = 0; i < count; i++) {
			Loot loot = hits[i].collider.GetComponent<Loot>();
			if (loot!=null) // && loot.IsInteractable()
				clickedItem = App.GetClosest<Loot>(clickedItem, loot);
		}
		return clickedAreas.Raycast(hits, count) || clickedItem != null;
	}

	#region SWIPE

	private void OnSwipeLeft() {
		camController.Rotate(-90f);
	}
	private void OnSwipeRight() {
		camController.Rotate(90f);
	}

	public float swipeThreshold = 40f;
	public float timeThreshold = 1f;

	public UnityEvent onSwipeLeft;
	public UnityEvent onSwipeRight;
	public UnityEvent onSwipeUp;
	public UnityEvent onSwipeDown;

	private Vector2 _fingerDown;
	private DateTime _fingerDownTime;
	private Vector2 _fingerUp;
	private DateTime _fingerUpTime;

	private void DetectGesture() {
		if (Input.GetMouseButtonDown(0)) {
			_fingerDown = Input.mousePosition;
			_fingerUp = Input.mousePosition;
			_fingerDownTime = DateTime.Now;
		}

		if (Input.GetMouseButtonUp(0)) {
			_fingerUp = Input.mousePosition;
			_fingerUpTime = DateTime.Now;
			CheckGesture();
		}

		foreach (var touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				_fingerDown = touch.position;
				_fingerUp = touch.position;
				_fingerDownTime = DateTime.Now;
			}

			if (touch.phase == TouchPhase.Ended) {
				_fingerUp = touch.position;
				_fingerUpTime = DateTime.Now;
				CheckGesture();
			}
		}
	}

	private void CheckGesture() {
		var duration = (float)_fingerUpTime.Subtract(_fingerDownTime).TotalSeconds;
		var dirVector = _fingerUp - _fingerDown;

		//if (duration > timeThreshold) return;
		if (dirVector.magnitude > swipeThreshold)
			DoSwipe(dirVector);
		else
			DoClick();

	}

	private void DoSwipe(Vector2 dirVector) {
		var direction = (Mathf.Atan(dirVector.y / dirVector.x) * Mathf.Rad2Deg);
		if (dirVector.x < 0)
			direction += 180;
		direction = (direction + 360) % 360;

		if (direction >= 45 && direction < 135)
			onSwipeUp.Invoke();
		else if (direction >= 135 && direction < 225)
			onSwipeLeft.Invoke();
		else if (direction >= 225 && direction < 315)
			onSwipeDown.Invoke();
		else
			onSwipeRight.Invoke();

	}

	private void DoClick() {
		if (App.uiManager.isClicOnUI) return;

		if (isClicOnObject()) {                                             // si on clique sur au moins 1 objet
																			//if (clickedItem && clickedItem.IsHighlightable()) {              //  si on clique sur un objet rémassable/utilisable
			if (clickedItem && clickedItem.IsInteractable()) {												//		si on est zoomé sur la zone
				if (!clickedItem.Act()) {                                      //			jouer l'action de l'objet
					clickedItem.parentTile?.CenterAndFocus(Zoom.tile);
					App.camController.Focus(Zoom.tile);
				} else {
					CLickOnArea();
				}
			} else {
				if (clickedItem && App.zoom != Zoom.tile)
					clickedItem.CenterAndFocus(Zoom.tile);
				else
					CLickOnArea();
			}
		} else if (!App.uiManager.isClicOnUI) {
			clickedAreas.Unselect();
			selectedItem = null;
			camController.Focus(Zoom.large);
		}
	}

	void CLickOnArea() {
		if (!clickedAreas.isEmpty) {
			if (clickedAreas.place) {
				if (!clickedAreas.place.Act()) {
				}
			} else if (clickedAreas.tile) {
				if (!clickedAreas.tile.Act()) {
				}
			} else if (clickedAreas.group) {
				if (clickedAreas.group != clickedAreas.selectedGroup) {
					clickedAreas.Select(clickedAreas.group);
				} else if (!clickedAreas.group.Act()) {
				}
			} else {
				App.camController.Focus(Zoom.large);
			}
		}
	}

	#endregion
}