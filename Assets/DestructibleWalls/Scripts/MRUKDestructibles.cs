using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.AI;
using Unity.AI.Navigation;



public class MRUKDestructibles : MonoBehaviour
{
    public GameObject PrefabToInstantiate;
    
    public void MRUKVonoroiGeneration(){
        GameObject effectMesh = GameObject.Find("GLOBAL_MESH_EffectMesh");
         
        if (effectMesh!= null)
        effectMesh.SetActive(false);

        Transform[] Transforms = FindObjectsOfType<Transform>();


        foreach(Transform transform in Transforms){
        if(transform.name.ToLower().Contains("floor_effectmesh"))
        {
            transform.gameObject.AddComponent<NavMeshSurface>().AddData();
        }
            if(transform.name.ToLower().Contains("ceiling_effectmesh") || transform.name.ToLower().Contains("wall_face_effectmesh"))
            {
                transform.gameObject.AddComponent<MRUKDestroyWalls>();
            }
        }

    }
}
