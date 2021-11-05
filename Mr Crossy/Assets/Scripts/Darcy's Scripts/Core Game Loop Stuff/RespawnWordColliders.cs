using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnWordColliders : MonoBehaviour
{
    List<BoxCollider> colliders = new List<BoxCollider>();

    void Start()
    {
        foreach(BoxCollider collider in gameObject.GetComponentsInChildren<BoxCollider>())
        {
            colliders.Add(collider);
        }
    }

    public void RespawnColliders()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            colliders[i].gameObject.SetActive(true);
        }
    }
}
