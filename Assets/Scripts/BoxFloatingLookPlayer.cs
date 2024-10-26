using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BoxFloatingLookPlayer : MonoBehaviour
{

    protected float initY;
    protected Transform player;

    void Start()
    {
        initY = transform.position.y;
        player = GameObject.Find("CenterEyeAnchor").transform;

    }

    // Update is called once per frame
    void Update()
    {

        float offset = Mathf.Sin(Time.time) * 0.05f;
        transform.position = new Vector3(transform.position.x, initY + offset, transform.position.z);

Vector3 direction = player.position - transform.position;
direction.y = 0; // Keep the direction strictly on the horizontal plane
Quaternion rotation = Quaternion.LookRotation(direction);
transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1f);
    }
}
