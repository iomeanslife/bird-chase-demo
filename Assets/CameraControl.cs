using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
      //  offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position + new Vector3(0,offset,0);
        transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
    }

    void FixedUpdate()
    {
     
    }
}
