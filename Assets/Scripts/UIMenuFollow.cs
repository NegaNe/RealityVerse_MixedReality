using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Oculus.Interaction.PoseDetection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UIMenuFollow : MonoBehaviour
{
public Transform Target;
public float OffsetX,OffsetY,OffsetZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new(Target.position.x + OffsetX, Target.position.y + OffsetY, Target.position.z + OffsetZ);
        transform.SetPositionAndRotation(Vector3.Slerp(transform.position, targetPosition, 1.8f * Time.deltaTime), Quaternion.identity);
    }
}
