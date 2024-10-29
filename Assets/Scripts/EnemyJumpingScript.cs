using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJumpingScript : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if agent is using OffMeshLink (i.e., a NavMeshLink)
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(JumpAcrossLink());
        }
    }

    private IEnumerator JumpAcrossLink()
    {
        // Get the start and end positions of the OffMeshLink
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = linkData.endPos;

        // Example "jump" movement using animation or custom movement
        float jumpDuration = 1.0f;  // Set duration of the jump
        float t = 0.0f;

        while (t < jumpDuration)
        {
            t += Time.deltaTime;
            float height = Mathf.Sin(Mathf.PI * t / jumpDuration); // Parabolic jump curve
            agent.transform.position = Vector3.Lerp(startPos, endPos, t / jumpDuration) + Vector3.up * height;
            yield return null;
        }

        // Complete the link
        agent.CompleteOffMeshLink();
    }
}
