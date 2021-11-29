using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DisableObject : MonoBehaviour
{
    public void Disable()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<Image>().color = Color.black;
    }
}
