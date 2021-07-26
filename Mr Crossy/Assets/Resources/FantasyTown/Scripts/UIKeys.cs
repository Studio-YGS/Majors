using UnityEngine;

/// <summary>
/// User interface keys. Taking care of key press events.
/// </summary>
public class UIKeys : MonoBehaviour {


	private ThemeManager tManager;


	/// <summary>
	/// Start this instance. Find a reference to the ThemeManager.
	/// </summary>
	void Start () {
		
		tManager = GameObject.FindObjectOfType<ThemeManager> ();
	}



	/// <summary>
	/// Update this instance. Check if key was pressed and inform the ThemeManager about it.
	/// </summary>
	void Update () {

		if (tManager == null)
			return;
		
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			tManager.setDayTimeSun ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			tManager.setDayTimeOvercast ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			tManager.setNightTime ();
		}

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            tManager.setHorror();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
			//Screen.fullScreen = false;
			//Cursor.visible = true;
		}


	}

}
