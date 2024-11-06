using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System;
using UnityEngine.XR.ARFoundation;

public class MRUKDestructibles : MonoBehaviour
{
    public GameObject PrefabToInstantiate;
    private List<NavMeshLinkInstance> linkInstances = new List<NavMeshLinkInstance>();

    private readonly string[] Classifications = {"WALL_ART", "WINDOW_FRAME", "COUCH", "TABLE", "BED", "LAMP", "PLANT", "SCREEN", "STORAGE", "OTHER"} ;
    
    public void MRUKVonoroiGeneration(){
        GameObject effectMesh = GameObject.Find("GLOBAL_MESH_EffectMesh");
         
        if (effectMesh!= null)
        effectMesh.SetActive(false);

        Transform[] Transforms = FindObjectsOfType<Transform>();


      foreach (Transform transform in Transforms)
        {

            foreach (var classification in Classifications)
            {
                if (transform.name.Contains(classification + "_EffectMesh")) // finds the objects
                {
                    transform.SetLayerRecursively(LayerMask.NameToLayer("Obstacles"));
                    NavMeshObstacle obstacle = transform.gameObject.AddComponent<NavMeshObstacle>();
                    obstacle.carving = true;

                    Bounds bounds = transform.GetComponent<Renderer>().bounds;

                    Vector3 startPosition = bounds.center + Vector3.left * bounds.extents.x; // Left edge
                    Vector3 endPosition = bounds.center + Vector3.right * bounds.extents.x;  // Right edge

                    NavMeshLinkData linkData = new NavMeshLinkData
                    {
                        startPosition = startPosition,
                        endPosition = endPosition,
                        width = bounds.size.z, 
                        costModifier = -1,    
                        bidirectional = true, 
                        area = 0              
                    };

                    NavMeshLinkInstance linkInstance = NavMesh.AddLink(linkData, transform.position, transform.rotation);
                    linkInstances.Add(linkInstance);
                }
            }
            if(transform.name.ToLower().Contains("floor_effectmesh"))
            {
                transform.gameObject.layer = LayerMask.NameToLayer("NonDestructible");
            }

            if(transform.name.ToLower().Contains("ceiling_effectmesh") || transform.name.ToLower().Contains("wall_face_effectmesh"))
            {
                transform.gameObject.AddComponent<MRUKDestroyWalls>();
            }
        }
        }


    }

