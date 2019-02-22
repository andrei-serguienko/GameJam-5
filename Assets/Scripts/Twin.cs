using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twin : MonoBehaviour
{
    public GameObject player;
    public float timeTwin;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeTwin)
            transform.position = player.GetComponent<Follow>().positionQueue.Dequeue();

        Facing();
    }

    private void Facing()
    {
            
    }
}
