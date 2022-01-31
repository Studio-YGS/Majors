using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectVariableHolder : MonoBehaviour
{
    public Transform hand;
    public Transform cam;
    public Player_Controller controller;
    public Image image;
    public TMP_Text textName;
    public Image imageTwo;
    public TMP_Text textNameTwo;
    public TMP_Text hoverText;
    public HeadBob headbob;

    private void Start()
    {
        headbob = FindObjectOfType<HeadBob>();
    }
}
