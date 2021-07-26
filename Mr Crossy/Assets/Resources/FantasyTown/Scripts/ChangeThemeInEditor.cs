using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper for changing the theme in the editor.
/// </summary>
public class ChangeThemeInEditor : MonoBehaviour {

    public bool DayTimeSun = true;
    public bool DayTimeOvercast = false;
    public bool NightTime = false;
    public bool Horror = false;

   
    /// <summary>
    /// Reference to the ThemeManager
    /// </summary>
    private ThemeManager tManager;


    /// <summary>
    /// Change the Theme on validate in the editor.
    /// </summary>
    void OnValidate() {
        tManager = GameObject.FindObjectOfType<ThemeManager>();
        if (tManager == null)
            return;
        tManager.assignComponents();
        
        if (DayTimeSun)
            tManager.setDayTimeSun();

        if (DayTimeOvercast)
            tManager.setDayTimeOvercast();

        if (NightTime)
            tManager.setNightTime();

        if (Horror)
            tManager.setHorror();
       
    }


}
