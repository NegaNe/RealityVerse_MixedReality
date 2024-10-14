using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class DestroyWallDebug : MonoBehaviour // doesnt work???
{
        
        [SerializeField] private LayerMask destroyableLayerMask;
        [SerializeField] private List<GameObject> debrisPrefabs;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private float destroyRadius = 0.05f;

        private Vector3 lastRayOrigin;
        private Vector3 lastRayDirection;

    // Start is called before the first frame update

            void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                DestroyObjectInPath();
            }
        }
       private void DestroyObjectInPath()
        {
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            Vector3 rayDirection = controllerRotation * Vector3.forward;

            lastRayOrigin = controllerPosition;
            lastRayDirection = rayDirection;
                Debug.DrawLine(controllerPosition, rayDirection, Color.blue);

            if (Physics.Raycast(lastRayOrigin, lastRayDirection, out RaycastHit hit, Mathf.Infinity, destroyableLayerMask))
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, destroyRadius, destroyableLayerMask);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Destructible"))
                    {
                        GameObject debrisPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
                        GameObject debris = Instantiate(debrisPrefab, hit.point, Quaternion.identity);
                        Destroy(hitCollider.gameObject);
                        Destroy(debris, 3f);

                        // AudioSource.PlayClipAtPoint(hitSound, hit.point); // sound klo destroyed
                    }
                }
            }
        }
}
