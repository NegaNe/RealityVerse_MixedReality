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

    public GameObject jumpableObject;  // Reference to the jumpable object prefab or GameObject
    public Transform startPoint;       // Position where the agent starts the jump
    public Transform endPoint;         // Position where the agent lands after the jump

    private List<NavMeshLinkInstance> linkInstances = new List<NavMeshLinkInstance>();

    private string[] Classifications = {"WALL_ART", "WINDOW_FRAME", "COUCH", "TABLE", "BED", "LAMP", "PLANT", "SCREEN", "STORAGE", "OTHER"} ;
    
    public void MRUKVonoroiGeneration(){
        GameObject effectMesh = GameObject.Find("GLOBAL_MESH_EffectMesh");
         
        if (effectMesh!= null)
        effectMesh.SetActive(false);

        Transform[] Transforms = FindObjectsOfType<Transform>();


      foreach (Transform transform in Transforms)
        {
            Semantics = transform.name.ToLower();

            foreach (var classification in Classifications)
            {
                if (transform.name.Contains(classification + "_EffectMesh"))
                {
                    // Add NavMeshObstacle to the object
                    NavMeshObstacle obstacle = transform.gameObject.AddComponent<NavMeshObstacle>();
                    obstacle.carving = true;

                    // Get the bounds of the object to determine link positions
                    Bounds bounds = transform.GetComponent<Renderer>().bounds;

                    // Define start and end positions based on bounds
                    Vector3 startPosition = bounds.center + Vector3.left * bounds.extents.x; // Left edge
                    Vector3 endPosition = bounds.center + Vector3.right * bounds.extents.x;  // Right edge

                    // Create and configure NavMeshLinkData
                    NavMeshLinkData linkData = new NavMeshLinkData
                    {
                        startPosition = startPosition,
                        endPosition = endPosition,
                        width = bounds.size.z, // Adjust link width to fit the object
                        costModifier = -1,    // Default cost
                        bidirectional = true, // Allow both directions if desired
                        area = 0              // Default area (Walkable)
                    };

                    // Add the link to the NavMesh and track the instance
                    NavMeshLinkInstance linkInstance = NavMesh.AddLink(linkData, transform.position, transform.rotation);
                    linkInstances.Add(linkInstance);

                    Debug.Log("NavMeshLink created for object: " + transform.name);
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

