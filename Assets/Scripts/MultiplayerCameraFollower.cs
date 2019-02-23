using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCameraFollower : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position;
        transform.position = new Vector3(pos.x, pos.y, -18);
    }
}
