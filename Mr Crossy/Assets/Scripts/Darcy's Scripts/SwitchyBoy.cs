using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchyBoy : MonoBehaviour
{
    [SerializeField]
    GameObject districtOne, districtTwo;

    PlayerRespawn playerRespawn;

    public UnityEvent switchTrigger;

    void Start()
    {
        playerRespawn = FindObjectOfType<PlayerRespawn>();
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
                    break;
                }
            case 2:
                {
                    districtOne.SetActive(false);
                    districtTwo.SetActive(true);
                    playerRespawn.SwitchRespawnPoint(whichController);
                    break;
                }
        }
    }
}
