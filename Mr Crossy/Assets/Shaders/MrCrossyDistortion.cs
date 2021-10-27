using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MrCrossyDistortion : MonoBehaviour
{
    public Material screenBlur;
    public Material motionBlur;
    public Volume volume;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            motionBlur.SetFloat("_BlurMagnitude", 0.01f);
            motionBlur.SetFloat("_ScreenXMagnitude", 0.02f);
            motionBlur.SetFloat("_ScreenYMagnitude", 0.02f);
            screenBlur.SetFloat("_Magnitude", 0.02f);

            Vignette vignette;
            if (volume.profile.TryGet<Vignette>(out vignette))
            {
                vignette.intensity.value = 0.7f;
            }

            ChromaticAberration aberration;
            if(volume.profile.TryGet<ChromaticAberration>(out aberration))
            {
                aberration.active = true;
            }
            
        }
    }

    private void OnApplicationQuit()
    {
        motionBlur.SetFloat("_BlurMagnitude", 0f);
        motionBlur.SetFloat("_ScreenXMagnitude", 0f);
        motionBlur.SetFloat("_ScreenYMagnitude", 0f);
        screenBlur.SetFloat("_Magnitude", 0f);
    }
}
