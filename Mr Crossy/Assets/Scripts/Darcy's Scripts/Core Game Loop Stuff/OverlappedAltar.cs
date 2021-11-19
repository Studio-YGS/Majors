using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlappedAltar : MonoBehaviour
{
    GameObject placedObject;
 
    public void ReceiveObject(GameObject sentObject)
    {
        placedObject = sentObject;
        SendToChildren();
    }

    public void ObjectPickedUp()
    {
        DetermineLetter[] childrenSend = GetComponentsInChildren<DetermineLetter>();

        for (int i = 0; i < childrenSend.Length; i++)
        {
            childrenSend[i].ObjectPickedUp();
        }
    }

    void SendToChildren()
    {
        DetermineLetter[] childrenSend = GetComponentsInChildren<DetermineLetter>();

        for(int i = 0; i < childrenSend.Length; i++)
        {
            childrenSend[i].ObjectPlaced(placedObject);
        }
    }
}
