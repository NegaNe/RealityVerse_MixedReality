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
        float offset = Mathf.Sin(Time.time * 0.5f) * 0.05f;
        transform.position = new Vector3(transform.position.x, initY + offset, transform.position.z);

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
    }
}
