using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseToggle : MonoBehaviour
{
    public GameObject houseInternals;

    public GameObject audSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            houseInternals.SetActive(true);
            OverseerController.m_PlayerInHouse = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            if(audSwitcher.GetComponent<ExternalInteralSwitch>().floors >=1)
            {
                houseInternals.SetActive(false);
                OverseerController.m_PlayerInHouse = false;
                audSwitcher.GetComponent<ExternalInteralSwitch>().floors = 0;
            }
        }
    }
}
