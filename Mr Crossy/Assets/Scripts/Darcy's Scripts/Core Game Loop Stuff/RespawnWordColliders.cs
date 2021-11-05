using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnWordColliders : MonoBehaviour
{
    [SerializeField]
    List<BoxCollider> colliders = new List<BoxCollider>();

    public void RespawnColliders()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            colliders[i].gameObject.SetActive(true);
        }
    }
}
