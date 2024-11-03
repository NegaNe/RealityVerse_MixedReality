using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MRUKDestroyWalls : MonoBehaviour
{
    private MRUKDestructibles destructible;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    void Start(){
        destructible = FindObjectOfType<MRUKDestructibles>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        CreateDestroyedPrefab();
    }

    //after delete, generate the walls
    private void CreateDestroyedPrefab(){
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        GameObject instantiatedPrefab = Instantiate(destructible.PrefabToInstantiate, transform);
        Bounds bounds = meshFilter.sharedMesh.bounds;

        float prefabInitialSizeX = 10f;
        float prefabInitialSizeZ = 10f;

        //walls size
        float scaleX = bounds.size.x / prefabInitialSizeX;
        float scaleZ = bounds.size.y / prefabInitialSizeZ;
        float scaleY = .001f;

        instantiatedPrefab.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        instantiatedPrefab.transform.localPosition = new Vector3(0, 0, 0);
        instantiatedPrefab.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        if(meshRenderer!=null) meshRenderer.enabled=false;
        if(meshCollider!=null) meshCollider.enabled=false;
    }

}
