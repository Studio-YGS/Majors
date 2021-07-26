using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBuilding : MonoBehaviour {

    bool inside = false;
    Transform character;
    private float initDist;
    private Light sunLight;
    private float currentAmbientIntensity;


    // Use this for initialization
    void Start() {

        GameObject sunGO = GameObject.FindGameObjectWithTag("SunLight");

        if (sunGO != null)
            sunLight = GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>();

    }

    private void OnTriggerExit(Collider other) {
        return;

        if (!other.CompareTag("GameController"))
            return;

        inside = false;
        GetComponentInParent<FrontDoor>().ExitBldg();

    }

    private void OnTriggerEnter(Collider other) {
        return;

        if (!other.CompareTag("GameController"))
            return;

        currentAmbientIntensity = RenderSettings.ambientIntensity;
        inside = true;
        character = other.transform;
        initDist = Vector3.Distance(transform.position, character.position);
    }

    public void Update() {
        if (inside) {
            float currentDist = Vector3.Distance(transform.position, character.position);

            float newIntensity = Mathf.Clamp(currentDist / initDist, currentAmbientIntensity / 2f, currentAmbientIntensity);
            RenderSettings.ambientIntensity = newIntensity;

            if (sunLight != null)
                sunLight.intensity = newIntensity;

        }
    }
}

