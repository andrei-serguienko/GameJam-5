using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twin : MonoBehaviour
{
    public GameObject player;
    public float timeTwin;

    private bool facingRight = true;
    Vector3 lastPosition;
    private Vector3 currentPosition;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPosition = transform.position;
        if (Time.time > timeTwin)
            transform.position = player.GetComponent<Follow>().positionQueue.Dequeue();

        Facing();

        lastPosition = currentPosition;
    }

    private void Facing()
    {
        if (lastPosition.x > currentPosition.x)
        {
            Flip("left");
        }
        else if (lastPosition.x < currentPosition.x)
        {
            Flip("right");
        }
    }
    
    void Flip(string arg)
    {
        if (arg == "left")
        {
            if (facingRight)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                facingRight = false;
            }
        } else if (arg == "right")
        {
            if (!facingRight)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                facingRight = true;
            }
        }
    }
}
