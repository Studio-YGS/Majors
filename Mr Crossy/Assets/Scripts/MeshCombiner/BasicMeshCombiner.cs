using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeshCombiner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CombineMeshes();
    }

    public void CombineMeshes()
    {
        Vector3 position = transform.position;
        Quaternion oldRot = transform.rotation;
        transform.position = Vector3.zero;

        transform.rotation = Quaternion.identity;


        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        Mesh finalMesh = new Mesh();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        
        for(int i = 0; i < meshFilters.Length; i++)
        {
            if(meshFilters[i].transform == transform)
            {
                continue;
            }
            combine[i].subMeshIndex = 0;
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            
        }

        finalMesh.CombineMeshes(combine);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        transform.position = position;
        transform.rotation = oldRot;
    }
}
