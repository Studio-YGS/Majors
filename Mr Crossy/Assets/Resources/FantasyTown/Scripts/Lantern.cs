using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lantern. Logic of the lantern.
/// </summary>
public class Lantern : MonoBehaviour {


    private Light lanternLight;


    /// <summary>
    /// Raises the enable event. Find the light component and enable/disable it depending on the choosen theme.
    /// </summary>
    void OnEnable() {

        lanternLight = transform.GetComponentInChildren<Light>();

        if (lanternLight == null)
            return;

        // enable for lantern inside a house  is called when a door is opened. Check if we switched to the night setting any time before.
        if (ThemeManager.CurrentTheme != ThemeManager.Theme.NightTime)
        	lanternLight.enabled = false;
    }

    /// <summary>
    /// Enables/Disables the light.
    /// </summary>
    /// <param name="lightOn">If set to <c>true</c> light on.</param>
    public void enableLight(bool lightOn) {
        
        if (lanternLight == null)
            return;

        // happen if called by ChangeThemeInEditor
        if (lanternLight == null)
            lanternLight = transform.GetComponentInChildren<Light>();

        if (lightOn) {
            lanternLight.enabled = true;
        }
        else {
            lanternLight.enabled = false;
        }
       
    }
}
