using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


/// <summary>
/// Theme manager. Contains the theme settings and change themes.
/// </summary>
public class ThemeManager : MonoBehaviour {

    public enum Theme { DayTimeSun, DayTimeOvercast, NightTime , Horror};
    public static Theme CurrentTheme;

    //public Material SkyMaterial;
    public Material[] SkyMaterials;
    public Material GlassEmissionMaterial;
    public PostProcessVolume[] PPVolumes;
    public ReflectionProbe[] ReflectionProbes;
    private Lantern[] Lanterns;
    private PostProcessLayer CamLayer;

    private Light sunLight;
    private Light moonLight;


    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable() {
        assignComponents();
        
    }

    /// <summary>
    /// Assigns the components.
    /// </summary>
    public void assignComponents() {
        Lanterns = GameObject.FindObjectsOfType<Lantern>();

        if (Camera.main != null)
            CamLayer = Camera.main.GetComponent<PostProcessLayer>();


        GameObject sunLightGO = GameObject.FindGameObjectWithTag("SunLight");

        if (sunLightGO != null)
            sunLight = sunLightGO.GetComponent<Light>();

        GameObject moonLightGO = GameObject.FindGameObjectWithTag("MoonLight");

        if (moonLightGO != null)
            moonLight = moonLightGO.GetComponent<Light>();

    }

    /// <summary>
    /// Settings for the daytime setting.
    /// </summary>
    public void setDayTimeSun() {

        if (CurrentTheme == Theme.DayTimeSun)
            return;

        for (int i = 0; i < PPVolumes.Length; i++) {
            PPVolumes[i].enabled = false;
        }
        PPVolumes[0].enabled = true;
        

        if (sunLight != null)
            sunLight.enabled = true;

        if (moonLight != null)
            moonLight.enabled = false;

        if(CamLayer != null)
            CamLayer.fog.excludeSkybox = true;

        RenderSettings.fogDensity = 0.012f;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.skybox = SkyMaterials[0];
        RenderSettings.fogColor = ColorConverter.HexToColor("808080FF");
        GlassEmissionMaterial.SetColor("_EmissionColor", new Color(0, 0, 0));
        enableLanterns(false);
        rebakeReflection();

        CurrentTheme = Theme.DayTimeSun;

    }

    /// <summary>
    /// Settings for the Daytime overcast settings.
    /// </summary>
    public void setDayTimeOvercast() {

        if (CurrentTheme == Theme.DayTimeOvercast)
            return;

        for (int i = 0; i < PPVolumes.Length; i++) {
            PPVolumes[i].enabled = false;
        }
        PPVolumes[1].enabled = true;
        
        if (sunLight != null)
            sunLight.enabled = false;

        if (moonLight != null)
            moonLight.enabled = false;

        if (CamLayer != null)
            CamLayer.fog.excludeSkybox = false;

        RenderSettings.fogDensity = 0.04f;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.skybox = SkyMaterials[1];
        RenderSettings.fogColor = ColorConverter.HexToColor("617076FF");
        enableLanterns(false);
        GlassEmissionMaterial.SetColor("_EmissionColor", new Color(0, 0, 0));
        rebakeReflection();

        CurrentTheme = Theme.DayTimeOvercast;
    }

    
    /// <summary>
    /// Settings for the Nighttime setting.
    /// </summary>
    public void setNightTime() {

        if (CurrentTheme == Theme.NightTime)
            return;
        for (int i = 0; i < PPVolumes.Length; i++) {
            PPVolumes[i].enabled = false;
        }
        PPVolumes[2].enabled = true;
        

        if (sunLight != null)
            sunLight.enabled = false;

        //if(moonLight != null)
        //	moonLight.enabled = true;

        if (CamLayer != null)
            CamLayer.fog.excludeSkybox = false;

        RenderSettings.fogDensity = 0.04f;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.ambientIntensity = 0.6f;
        RenderSettings.skybox = SkyMaterials[2];
        RenderSettings.fogColor = ColorConverter.HexToColor("2F3438FF");
        enableLanterns(true);
        GlassEmissionMaterial.SetColor("_EmissionColor", new Color(2.5f, 2.3f, 1.8f));
        rebakeReflection();

        CurrentTheme = Theme.NightTime;
    }

    /// <summary>
    /// Settings for the Mystic settings.
    /// </summary>
    public void setHorror() {

        if (CurrentTheme == Theme.Horror)
            return;

        for (int i = 0; i < PPVolumes.Length; i++) {
            PPVolumes[i].enabled = false;
        }
        PPVolumes[3].enabled = true;
        
        if (sunLight != null)
            sunLight.enabled = false;

        if (moonLight != null)
            moonLight.enabled = false;

        if (CamLayer != null)
            CamLayer.fog.excludeSkybox = true;

        RenderSettings.skybox = SkyMaterials[3];
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.02f;
        RenderSettings.ambientIntensity = 1.5f;
        RenderSettings.fogColor = ColorConverter.HexToColor("404040FF");
        enableLanterns(false);
        GlassEmissionMaterial.SetColor("_EmissionColor", new Color(0, 0, 0));
        rebakeReflection();

        CurrentTheme = Theme.Horror;
    }

    /// <summary>
    /// Rebakes the reflection.
    /// </summary>
    private void rebakeReflection() {
        for (int i = 0; i < ReflectionProbes.Length; i++) {
            ReflectionProbes[i].RenderProbe();
        }
    }

    /// <summary>
    /// Enables the lanterns.
    /// </summary>
    /// <param name="lanternEnabled">If set to <c>true</c> lantern enabled.</param>
    private void enableLanterns(bool lanternEnabled) {
        for (int i = 0; i < Lanterns.Length; i++) {
            Lanterns[i].enableLight(lanternEnabled);
        }
    }
}
