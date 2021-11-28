using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MrCrossyDistortion : MonoBehaviour
{
    public Material screenBlur;
    public Material motionBlur;
    public VolumeProfile[] volume;
    public GameObject mask;
    public Vignette[] vignette /*= new Vignette[2]*/;
    public ChromaticAberration[] aberration /*= new ChromaticAberration[2]*/;
    public ColorAdjustments[] colorAdjustments /*= new ColorAdjustments[2]*/;
    public float baseVignette = 0.4f;
    public float vignetteIncreaseRate;
    bool vignetteReducing;

    public float insanityIncreaseRate;
    public float distInsanityIncreaseRate;
    public GameObject mrCrossy;
    Transform player;
    public bool increasingInsanity;
    public bool reducingInsanity;
    public int hits;
    private bool healing;
    void Start()
    {
        for (int i = 0; i < volume.Length; i++)
        {
            volume[i].TryGet<ChromaticAberration>(out aberration[i]);
            volume[i].TryGet<Vignette>(out vignette[i]);
            volume[i].TryGet<ColorAdjustments>(out colorAdjustments[i]);
        }
        player = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            HitByCrossy();
        }

        if (mrCrossy != null)
        {
            float distance = Vector3.Distance(player.position, mrCrossy.transform.position);
            if (increasingInsanity)
            {
                //Mathf.Lerp(motionBlur.GetFloat("_ScreenXMagnitude"), 0.025f / ((distance / 20) * (distance / 20)), 0.025f);
                if (motionBlur.GetFloat("_BlurMagnitude") < 0.00175f)
                {
                    motionBlur.SetFloat("_BlurMagnitude", Mathf.Lerp(motionBlur.GetFloat("_BlurMagnitude"), 0.0035f / ((distance / 20) * (distance / 20)), 0.025f));
                }
                if (motionBlur.GetFloat("_ScreenXMagnitude") < 0.0125f)
                {
                    motionBlur.SetFloat("_ScreenXMagnitude", Mathf.Lerp(motionBlur.GetFloat("_ScreenXMagnitude"), 0.025f / ((distance / 20) * (distance / 20)), 0.025f));
                }
                if (motionBlur.GetFloat("_ScreenYMagnitude") < 0.0125f)
                {
                    motionBlur.SetFloat("_ScreenYMagnitude", Mathf.Lerp(motionBlur.GetFloat("_ScreenYMagnitude"), 0.025f / ((distance / 20) * (distance / 20)), 0.025f));
                }
                if (screenBlur.GetFloat("_Magnitude") < 0.005f)
                {
                    screenBlur.SetFloat("_Magnitude", Mathf.Lerp(screenBlur.GetFloat("_Magnitude"), 0.01f / ((distance / 20) * (distance / 20)), 0.025f));
                }

                /*if (motionBlur.GetFloat("_BlurMagnitude") < 0.007f / (distance/2))
                {
                    motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") + Time.deltaTime * distInsanityIncreaseRate);
                }
                else if (motionBlur.GetFloat("_BlurMagnitude") > 0.007f / (distance / 2))
                {
                    motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") - Time.deltaTime * (distInsanityIncreaseRate / 10));
                }*/


                //if (motionBlur.GetFloat("_ScreenXMagnitude") < 0.025f / (distance / 2))
                //{
                //    motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") + Time.deltaTime * distInsanityIncreaseRate);
                //}
                //else if (motionBlur.GetFloat("_ScreenXMagnitude") > 0.025f / (distance / 2))
                //{
                //    motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") - Time.deltaTime * (distInsanityIncreaseRate / 10));
                //}


                //if (motionBlur.GetFloat("_ScreenYMagnitude") < 0.025f / (distance / 2))
                //{
                //    motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") + Time.deltaTime * distInsanityIncreaseRate);
                //}
                //else if (motionBlur.GetFloat("_ScreenYMagnitude") > 0.025f / (distance / 2))
                //{
                //    motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") - Time.deltaTime * (distInsanityIncreaseRate / 10));
                //}


                //if (screenBlur.GetFloat("_Magnitude") < 0.02f / (distance / 2))
                //{
                //    screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") + Time.deltaTime * distInsanityIncreaseRate);
                //}
                //else if (screenBlur.GetFloat("_Magnitude") > 0.02f / (distance / 2))
                //{
                //    screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") - Time.deltaTime * (distInsanityIncreaseRate / 10));
                //}

                for (int i = 0; i < volume.Length; i++)
                {
                    aberration[i].active = true;
                    if (aberration[i].intensity.value < 0.5f / (distance / 2))
                    {
                        aberration[i].intensity.value += Time.deltaTime * (distInsanityIncreaseRate * 100f);
                    }
                    else if (aberration[i].intensity.value > 0.5f / (distance / 2))
                    {
                        aberration[i].intensity.value -= Time.deltaTime * (distInsanityIncreaseRate);
                    }
                }

            }


        }
        if (reducingInsanity)
        {
            if (motionBlur.GetFloat("_BlurMagnitude") > 0)
            {
                motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") - Time.unscaledDeltaTime * (distInsanityIncreaseRate / 10));
            }



            if (motionBlur.GetFloat("_ScreenXMagnitude") > 0)
            {
                motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") - Time.unscaledDeltaTime * (distInsanityIncreaseRate / 10));
            }



            if (motionBlur.GetFloat("_ScreenYMagnitude") > 0)
            {
                motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") - Time.unscaledDeltaTime * (distInsanityIncreaseRate / 10));
            }



            if (screenBlur.GetFloat("_Magnitude") > 0)
            {
                screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") - Time.unscaledDeltaTime * (distInsanityIncreaseRate / 10));
            }

            for (int i = 0; i < volume.Length; i++)
            {
                if (aberration[i].intensity.value > 0)
                {
                    aberration[i].intensity.value -= Time.unscaledDeltaTime * (distInsanityIncreaseRate * 10);
                }

                if (aberration[i].intensity.value <= 0 && screenBlur.GetFloat("_Magnitude") <= 0 && motionBlur.GetFloat("_ScreenYMagnitude") <= 0
                    && motionBlur.GetFloat("_ScreenXMagnitude") <= 0 && motionBlur.GetFloat("_BlurMagnitude") <= 0)
                {
                    motionBlur.SetFloat("_BlurMagnitude", 0f);
                    motionBlur.SetFloat("_ScreenXMagnitude", 0f);
                    motionBlur.SetFloat("_ScreenYMagnitude", 0f);
                    screenBlur.SetFloat("_Magnitude", 0f);
                    aberration[i].active = false;
                    mrCrossy = null;
                    increasingInsanity = false;
                    reducingInsanity = false;
                }
            }

        }

    }

    private void OnApplicationQuit()
    {
        motionBlur.SetFloat("_BlurMagnitude", 0f);
        motionBlur.SetFloat("_ScreenXMagnitude", 0f);
        motionBlur.SetFloat("_ScreenYMagnitude", 0f);
        screenBlur.SetFloat("_Magnitude", 0f);
        for (int i = 0; i < volume.Length; i++)
        {
            vignette[i].color.value = Color.red;
            vignette[i].intensity.value = 0;
            colorAdjustments[i].colorFilter.value = Color.white;
        }
    }

    public void IncreaseInsanity(GameObject crossy)
    {
        mrCrossy = crossy;
        increasingInsanity = true;
    }

    public void ReduceInsanity()
    {
        Debug.Log("SANE Reducy");
        increasingInsanity = false;
        reducingInsanity = true;
    }

    public void LerpInsanity()
    {
        if (motionBlur.GetFloat("_BlurMagnitude") < 0.00175f)
        {
            motionBlur.SetFloat("_BlurMagnitude", motionBlur.GetFloat("_BlurMagnitude") + Time.deltaTime * insanityIncreaseRate);
        }
        if (motionBlur.GetFloat("_ScreenXMagnitude") < 0.0125f)
        {
            motionBlur.SetFloat("_ScreenXMagnitude", motionBlur.GetFloat("_ScreenXMagnitude") + Time.deltaTime * insanityIncreaseRate);
        }
        if (motionBlur.GetFloat("_ScreenYMagnitude") < 0.0125f)
        {
            motionBlur.SetFloat("_ScreenYMagnitude", motionBlur.GetFloat("_ScreenYMagnitude") + Time.deltaTime * insanityIncreaseRate);
        }
        if (screenBlur.GetFloat("_Magnitude") < 0.005f)
        {
            screenBlur.SetFloat("_Magnitude", screenBlur.GetFloat("_Magnitude") + Time.deltaTime * insanityIncreaseRate);
        }

        for (int i = 0; i < volume.Length; i++)
        {
            aberration[i].active = true;
            if (aberration[i].intensity.value < 0.5f)
            {
                aberration[i].intensity.value += Time.deltaTime * (insanityIncreaseRate * 100f);
            }
        }
    }

    public void IncreaseVignette()
    {
        if (vignetteReducing)
        {
            StopCoroutine("ReduceVignette");
            vignetteReducing = false;
        }
        for (int i = 0; i < volume.Length; i++)
        {
            //if (vignette[i].intensity.value < 1)
            //{
            //    //mask.SetActive(true);
            //    vignette[i].intensity.value += Time.deltaTime * vignetteIncreaseRate;
            //}
            //colorAdjustments.active = true;
            if (colorAdjustments[i].colorFilter.value != Color.black)
            {
                colorAdjustments[i].colorFilter.value = Color.Lerp(colorAdjustments[i].colorFilter.value, Color.black, 0.04f);
            }
        }


    }

    public void DistanceVignette(GameObject crossy)
    {
        float distance = Vector3.Distance(player.position, crossy.transform.position);
        //colorAdjustments.active = true;
        for (int i = 0; i < volume.Length; i++)
        {
            colorAdjustments[i].colorFilter.value = Color.Lerp(colorAdjustments[i].colorFilter.value, Color.black + Color.white / Mathf.Clamp((10 / distance), 1, 10), 0.07f);
            //vignette[i].intensity.value += Time.deltaTime * vignetteIncreaseRate / (distance / 4);
        }

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
        while (/*vignette[0].intensity.value > baseVignette &&*/ colorAdjustments[0].colorFilter.value != Color.white)
        {
            for (int i = 0; i < volume.Length; i++)
            {
                //vignette[i].intensity.value -= Time.deltaTime * vignetteIncreaseRate;
                colorAdjustments[i].colorFilter.value = Color.Lerp(colorAdjustments[i].colorFilter.value, Color.white, 0.2f);

            }

            yield return null;
        }
        for (int i = 0; i < volume.Length; i++)
        {
            //vignette[i].intensity.value = baseVignette;
            colorAdjustments[i].colorFilter.value = Color.white;
        }

        //colorAdjustments.active = false;
        vignetteReducing = false;
    }

    public void DarkenScreen(float speed)
    {
        StartCoroutine(BlackenScreen(speed));

    }

    IEnumerator BlackenScreen(float speed)
    {

        StopCoroutine("ReduceVignette");
        //colorAdjustments.active = true;
        float time = 0;
        while (time < 3)
        {
            time += Time.unscaledDeltaTime;
            for (int i = 0; i < volume.Length; i++)
            {
                //vignette[i].intensity.value += Time.deltaTime * vignetteIncreaseRate * speed;
                colorAdjustments[i].colorFilter.value = Color.Lerp(colorAdjustments[i].colorFilter.value, Color.black, 0.1f);
            }

            yield return null;
        }
        mask.SetActive(true);
        //for (int i = 0; i < volume.Length; i++)
        //{
        //    colorAdjustments[i].colorFilter.value = Color.white;
        //    vignette[i].intensity.value = baseVignette;
        //}
        //colorAdjustments.active = false;
    }

    public void WhitenScreen(float speed)
    {
        StartCoroutine(BrightenScreen(speed));
    }
    IEnumerator BrightenScreen(float speed)
    {
        mask.SetActive(false);
        while (/*vignette[0].intensity.value > baseVignette &&*/ colorAdjustments[0].colorFilter.value != Color.white)
        {
            for (int i = 0; i < volume.Length; i++)
            {
                //vignette[i].intensity.value -= Time.deltaTime * vignetteIncreaseRate * speed;
                colorAdjustments[i].colorFilter.value = Color.Lerp(colorAdjustments[i].colorFilter.value, Color.white, 0.02f);
            }

            yield return null;
        }
        for (int i = 0; i < volume.Length; i++)
        {
            //vignette[i].intensity.value = baseVignette;
            colorAdjustments[i].colorFilter.value = Color.white;
        }
    }

    public void HitByCrossy()
    {
        if (healing) { StopCoroutine("ReduceDamage"); healing = false; }
        hits += 1;
        StartCoroutine("TakeDamage");
    }

    public void ResetDamage()
    {
        for (int i = 0; i < volume.Length; i++)
        {
            //vignette[i].color.value = new Color32(204, 16, 16, 1);
            vignette[i].color.value = Color.red;
            vignette[i].intensity.value = 0;
        }
        hits = 0;
    }

    IEnumerator TakeDamage()
    {
        if (hits == 1)
        {
            while (vignette[0].intensity.value < 0.4)
            {
                for (int i = 0; i < volume.Length; i++)
                {
                    vignette[i].intensity.value = Mathf.Lerp(vignette[i].intensity.value, 0.4f, 0.07f);
                }

                yield return null;
            }
        }
        else if (hits == 2)
        {
            while (vignette[0].intensity.value < 0.6)
            {
                for (int i = 0; i < volume.Length; i++)
                {
                    vignette[i].intensity.value = Mathf.Lerp(vignette[i].intensity.value, 0.61f, 0.07f);
                }

                yield return null;
            }
        }
        else if (hits >= 3)
        {
            while (vignette[0].intensity.value < 0.6 && vignette[0].color.value != Color.red)
            {
                for (int i = 0; i < volume.Length; i++)
                {
                    vignette[i].intensity.value = Mathf.Lerp(vignette[i].intensity.value, 0.61f, 0.07f);
                    vignette[i].color.value = Color.Lerp(vignette[i].color.value, Color.red, 0.07f);
                }

                yield return null;
            }
        }
    }

    public void lowerDamage()
    {
        if (!healing) StartCoroutine("ReduceDamage");
    }

    IEnumerator ReduceDamage()
    {
        healing = true;
        while (vignette[0].intensity.value >= 0)
        {
            for (int i = 0; i < volume.Length; i++)
            {
                vignette[i].intensity.value = Mathf.Lerp(vignette[i].intensity.value, 0f, 0.04f);
                //vignette[i].color.value = Color.Lerp(vignette[i].color.value, new Color32(204, 16, 16, 1), 0.07f);
            }

            yield return null;
        }
        for (int i = 0; i < volume.Length; i++)
        {
            //vignette[i].color.value = new Color32(204, 16, 16, 1);
            vignette[i].intensity.value = 0;
        }
        hits = 0;
        healing = false;
    }
    public void ShoobyDooby()
    {
        if (OverseerController.ObserverTree)
        {
            TreeMalarkey.RegisterEventOnTree(OverseerController.ObserverTree, "Healing", lowerDamage);
        }
        if (CrossyController.crossyTree)
        {
            TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "CrossyHit", HitByCrossy);
        }
    }

    private void OnDisable()
    {
        if (OverseerController.ObserverTree)
        {
            TreeMalarkey.UnregisterEventOnTree(OverseerController.ObserverTree, "Healing", lowerDamage);
        }
        if (CrossyController.crossyTree)
        {
            TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "CrossyHit", HitByCrossy);
        }
    }
}
