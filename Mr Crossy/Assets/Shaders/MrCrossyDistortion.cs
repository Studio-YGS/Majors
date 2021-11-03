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
    public GameObject mask;
    [HideInInspector] public Vignette vignette;
    [HideInInspector] public ChromaticAberration aberration;
    [HideInInspector] public ColorAdjustments colorAdjustments;
    public float baseVignette = 0.4f;
    public float vignetteIncreaseRate;
    bool vignetteReducing;

    public float insanityIncreaseRate;
    public GameObject mrCrossy;
    Transform player;
    public bool increasingInsanity;
    public bool reducingInsanity;
    void Start()
    {
        volume.profile.TryGet<ChromaticAberration>(out aberration);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        player = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(mrCrossy != null)
        {
            float distance = Vector3.Distance(player.position, mrCrossy.transform.position);
            if (increasingInsanity)
            {
                if (motionBlur.GetFloat("_BlurMagnitude") < 0.007f / (distance/2))
                {
                    motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") + Time.deltaTime * insanityIncreaseRate);
                }
                else if (motionBlur.GetFloat("_BlurMagnitude") > 0.007f / (distance / 2))
                {
                    motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
                }


                if (motionBlur.GetFloat("_ScreenXMagnitude") < 0.025f / (distance / 2))
                {
                    motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") + Time.deltaTime * insanityIncreaseRate);
                }
                else if (motionBlur.GetFloat("_ScreenXMagnitude") > 0.025f / (distance / 2))
                {
                    motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
                }


                if (motionBlur.GetFloat("_ScreenYMagnitude") < 0.025f / (distance / 2))
                {
                    motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") + Time.deltaTime * insanityIncreaseRate);
                }
                else if (motionBlur.GetFloat("_ScreenYMagnitude") > 0.025f / (distance / 2))
                {
                    motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
                }


                if (screenBlur.GetFloat("_Magnitude") < 0.02f / (distance / 2))
                {
                    screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") + Time.deltaTime * insanityIncreaseRate);
                }
                else if (screenBlur.GetFloat("_Magnitude") > 0.02f / (distance / 2))
                {
                    screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
                }


                aberration.active = true;
                if (aberration.intensity.value < 1 / (distance / 2))
                {
                    aberration.intensity.value += Time.deltaTime * (insanityIncreaseRate * 100f);
                }
                else if (aberration.intensity.value > 1 / (distance / 2))
                {
                    aberration.intensity.value -= Time.deltaTime * (insanityIncreaseRate);
                }
            }

            
        }
        if (reducingInsanity)
        {
            if (motionBlur.GetFloat("_BlurMagnitude") > 0)
            {
                motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
            }



            if (motionBlur.GetFloat("_ScreenXMagnitude") > 0)
            {
                motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
            }



            if (motionBlur.GetFloat("_ScreenYMagnitude") > 0)
            {
                motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
            }



            if (screenBlur.GetFloat("_Magnitude") > 0)
            {
                screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") - Time.deltaTime * (insanityIncreaseRate / 10));
            }


            if (aberration.intensity.value > 0)
            {
                aberration.intensity.value -= Time.deltaTime * (insanityIncreaseRate * 10);
            }

            if (aberration.intensity.value <= 0 && screenBlur.GetFloat("_Magnitude") <= 0 && motionBlur.GetFloat("_ScreenYMagnitude") <= 0
                && motionBlur.GetFloat("_ScreenXMagnitude") <= 0 && motionBlur.GetFloat("_BlurMagnitude") <= 0)
            {
                motionBlur.SetFloat("_BlurMagnitude", 0f);
                motionBlur.SetFloat("_ScreenXMagnitude", 0f);
                motionBlur.SetFloat("_ScreenYMagnitude", 0f);
                screenBlur.SetFloat("_Magnitude", 0f);
                aberration.active = false;
                mrCrossy = null;
                increasingInsanity = false;
                reducingInsanity = false;
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

    public void IncreaseInsanity(GameObject crossy)
    {
        mrCrossy = crossy;
        increasingInsanity = true;
    }

    public void ReduceInsanity()
    {
        increasingInsanity = false;
        reducingInsanity = true;
    }

    public void IncreaseVignette()
    {
        if (vignetteReducing)
        {
            StopCoroutine("ReduceVignette");
            vignetteReducing = false;
        }
        if(vignette.intensity.value < 1)
        {
            //mask.SetActive(true);
            vignette.intensity.value += Time.deltaTime * vignetteIncreaseRate;
        }
        colorAdjustments.active = true;
        if(colorAdjustments.colorFilter.value != Color.black)
        {
            colorAdjustments.colorFilter.value = new Color (colorAdjustments.colorFilter.value.r - Time.deltaTime * vignetteIncreaseRate, 
                colorAdjustments.colorFilter.value.b - Time.deltaTime * vignetteIncreaseRate, colorAdjustments.colorFilter.value.g - Time.deltaTime * vignetteIncreaseRate);
        }
        
    }

    public void DistanceVignette(GameObject crossy)
    {
        float distance = Vector3.Distance(player.position, crossy.transform.position);
        colorAdjustments.active = true;
        colorAdjustments.colorFilter.value = new Color(colorAdjustments.colorFilter.value.r - (Time.unscaledDeltaTime * vignetteIncreaseRate / (distance)),
                colorAdjustments.colorFilter.value.b - (Time.unscaledDeltaTime * vignetteIncreaseRate / (distance )), colorAdjustments.colorFilter.value.g - (Time.unscaledDeltaTime * vignetteIncreaseRate / (distance )));
        vignette.intensity.value += Time.deltaTime * vignetteIncreaseRate / (distance / 4);
    }


    public void DecreaseVignette()
    {
        if (!vignetteReducing)
        {
            StartCoroutine("ReduceVignette");
        }
    }

    IEnumerator ReduceVignette()
    {
        vignetteReducing = true;
        //mask.SetActive(false);
        while(vignette.intensity.value > baseVignette && colorAdjustments.colorFilter.value != new Color (255,255,255))
        {
            vignette.intensity.value -= Time.deltaTime * vignetteIncreaseRate;
            colorAdjustments.colorFilter.value = new Color(colorAdjustments.colorFilter.value.r + Time.deltaTime * vignetteIncreaseRate,
                colorAdjustments.colorFilter.value.b + Time.deltaTime * vignetteIncreaseRate, colorAdjustments.colorFilter.value.g + Time.deltaTime * vignetteIncreaseRate);
            yield return null;
        }
        vignette.intensity.value = baseVignette;
        colorAdjustments.active = false;
        vignetteReducing = false;
    }

    public void DarkenScreen(float speed)
    {
        StartCoroutine(BlackenScreen(speed));
    }

    IEnumerator BlackenScreen(float speed)
    {
        
        StopCoroutine("ReduceVignette");
        colorAdjustments.active = true;
        while (colorAdjustments.colorFilter.value != new Color(0, 0, 0))
        {
            vignette.intensity.value += Time.deltaTime * vignetteIncreaseRate * speed;
            colorAdjustments.colorFilter.value = new Color(colorAdjustments.colorFilter.value.r - Time.deltaTime * vignetteIncreaseRate * speed,
                colorAdjustments.colorFilter.value.b - Time.deltaTime * vignetteIncreaseRate * speed, colorAdjustments.colorFilter.value.g - Time.deltaTime * vignetteIncreaseRate * speed);
            yield return null;
        }
        colorAdjustments.active = false;
    }
}
