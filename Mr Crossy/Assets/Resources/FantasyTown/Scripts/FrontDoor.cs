using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Front door. Contains the logic of the door of a building. Takes care of enable/disable the interiors of a house and adapt the lighting.
/// If a player enters a house the following things happen: 
/// - Enable the interior of this building.
/// - Enable the reflection probes inside this building.
/// - Enable the transparent meshes ("the glass") of the windows. 
/// - Dim the sunlight and the environment light slightly.
/// </summary>
public class FrontDoor : MonoBehaviour {

	public Transform Building;
	public GameObject RenderComponents;
	public GameObject Interior;
	public Transform DoorWing;
	public bool invertDoorOpeningDirection = false;


	private bool openDoor = false;
	private bool closeDoor = false;
	private float timeCount = 0f;
	private bool insideDoorTrigger = false;

    private GameObject PressE;
    private List<Transform> windowsInside;
	private Light moonLight;
	private Quaternion doorOpenRotation = Quaternion.Euler(new Vector3(0f,100f,0f));
	private Quaternion doorCloseRotation = Quaternion.Euler(new Vector3(0f,0f,0f));



	/// <summary>
	/// Raises the enable event. Set up references. Find the "inside glass" meshes of the window. 
	/// </summary>
	void OnEnable() {
		
		GameObject moonlightGO = GameObject.FindGameObjectWithTag ("MoonLight");
        PressE = GameObject.Find("(E)OpenFrontDoor");

		if(moonlightGO != null)
			moonLight = GameObject.FindGameObjectWithTag ("MoonLight").GetComponent<Light> ();

		windowsInside = new List<Transform> ();

		if (Building != null) {

			Transform[] blgChildren = Building.GetComponentsInChildren<Transform> ();

			for (int i = 0; i < blgChildren.Length; i++) {

				if (blgChildren [i].tag.Equals ("GlassInside")) {
					windowsInside.Add (blgChildren [i]);
				}
			}
		}
	}

	/// <summary>
	/// Start this instance. Disable the "Press E" UI hint. Disable the interior, the lights and transparent parts (glass) of the windows  of this building.
	/// </summary>
	void Start () {

		if(PressE != null)
			PressE.SetActive(false);

		if(Interior != null)
			Interior.SetActive (false);

		foreach (Transform window in windowsInside) {
			window.gameObject.SetActive (false);
		}

		if(RenderComponents != null)
			RenderComponents.SetActive (false);

		doorCloseRotation = DoorWing.rotation;
	
		float y = doorCloseRotation.eulerAngles.y + (100f * (invertDoorOpeningDirection ? 1 : -1));

		doorOpenRotation = Quaternion.Euler(new Vector3(0f,y,0f));


	}
	
	/// <summary>
	/// Update this instance. Check if "E" was pressed to open the door of a building or if the player left the door area and animates the door accordingly.
	/// </summary>
	void Update () {

		if (Input.GetKeyDown (KeyCode.E) && insideDoorTrigger) {

			openDoor = true;

			if(Interior != null)
				Interior.SetActive (true);

			if(RenderComponents != null)
				RenderComponents.SetActive (true);


			foreach (Transform window in windowsInside) {
				window.gameObject.SetActive (true);
			}



		}

		if (openDoor) {

			
			closeDoor = false;

			if (Mathf.Abs(Mathf.DeltaAngle(DoorWing.rotation.eulerAngles.y, doorOpenRotation.eulerAngles.y)) >0.1f) {
				DoorWing.rotation = Quaternion.Slerp (doorCloseRotation, doorOpenRotation, timeCount);
				timeCount = timeCount + Time.deltaTime;

			} else {
				timeCount = 0f;
				openDoor = false;

			}

		}

		if (closeDoor) {
			openDoor = false;
			if (Mathf.Abs(Mathf.DeltaAngle(DoorWing.rotation.eulerAngles.y, doorCloseRotation.eulerAngles.y)) >0.1f) {
				DoorWing.rotation = Quaternion.Slerp (doorOpenRotation, doorCloseRotation, timeCount);
				timeCount = timeCount + Time.deltaTime;

			} else {
				timeCount = 0f;
				closeDoor = false;
			}
		}
		
	}

	/// <summary>
	/// Disable all interior components if the player leaves a building.
	/// </summary>
	public void ExitBldg() {
		if(RenderComponents != null)
			RenderComponents.SetActive (false);

		foreach (Transform window in windowsInside) {
			window.gameObject.SetActive (false);
		}

		if(Interior != null)
			Interior.SetActive (false);

	}

	/// <summary>
	/// Called if the player enters the door area.
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerEnter(Collider other) {

		if(PressE != null)
			PressE.SetActive(true);

		insideDoorTrigger = true;
	}

	/// <summary>
	/// Called if the player leaves the door area.
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerExit(Collider other) {
		if(PressE != null)
			PressE.SetActive(false);
		
		closeDoor = true;
		openDoor = false;
		insideDoorTrigger = false;
	}




}
