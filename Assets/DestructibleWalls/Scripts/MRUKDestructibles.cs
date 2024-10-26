using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System;



public class MRUKDestructibles : MonoBehaviour
{
    public GameObject PrefabToInstantiate;
    private string Semantics;
    private readonly string[] Classifications = {"WALL_ART", "WALL_FACE", "WINDOW_FRAME", "COUCH", "TABLE", "BED", "LAMP", "PLANT", "SCREEN", "STORAGE", "OTHER"} ;
    
    public void MRUKVonoroiGeneration(){
        GameObject effectMesh = GameObject.Find("GLOBAL_MESH_EffectMesh");
         
        if (effectMesh!= null)
        effectMesh.SetActive(false);

        Transform[] Transforms = FindObjectsOfType<Transform>();


        foreach(Transform transform in Transforms){
        Semantics = transform.name.ToLower();

        foreach(var semantic_classfications in Classifications)
        {
            if(transform.name.Contains(semantic_classfications +"_EffectMesh"))
            {
                transform.gameObject.AddComponent<NavMeshObstacle>();
            }
        }
            if(Semantics.Contains("floor_effectmesh"))
            {
                transform.gameObject.AddComponent<NavMeshModifier>();
            }

            if(transform.name.ToLower().Contains("ceiling_effectmesh") || transform.name.ToLower().Contains("wall_face_effectmesh"))
            {
                transform.gameObject.AddComponent<MRUKDestroyWalls>();
            }
        }

    }
}
