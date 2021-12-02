using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChildCombiner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CombineMeshes();
    }

    public void CombineMeshes()
    {
        //Vector3 position = transform.position;
        //transform.position = Vector3.zero;


        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 1;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
        transform.gameObject.SetActive(true);

        //transform.position = position;
    }
}
