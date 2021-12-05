using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchyBoy : MonoBehaviour
{
    [SerializeField]
    GameObject districtOne, districtTwo;

    PlayerRespawn playerRespawn;
    OverseerController seer;

    public UnityEvent switchTrigger;

    void Start()
    {
        playerRespawn = FindObjectOfType<PlayerRespawn>();
        seer = FindObjectOfType<OverseerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        switchTrigger.Invoke();
    }

    public void ControllerSwitch(int whichController)
    {
        switch (whichController)
        {
            case 1:
                {
                    districtOne.SetActive(true);
                    districtTwo.SetActive(false);
                    playerRespawn.SwitchRespawnPoint(whichController);
                    seer.SetLighthouseGroup(whichController);
                    break;
                }
            case 2:
                {
                    districtOne.SetActive(false);
                    districtTwo.SetActive(true);
                    playerRespawn.SwitchRespawnPoint(whichController);
                    seer.SetLighthouseGroup(whichController);
                    break;
                }
        }
    }
}
