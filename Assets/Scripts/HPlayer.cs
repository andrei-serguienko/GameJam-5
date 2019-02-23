using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HPlayer : MonoBehaviour
{
    public float speed;
    public float timeBetweenAttack;
    public Queue<String> movesQueue = new Queue<String>();
    public Queue<Vector3> positionQueue = new Queue<Vector3>();
    private bool jumping = false;
    private bool attack = false;
    private bool facingRight = true;
    public int timeConstant = 500;

    public int forceJump;

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
        }
        else if (arg == "right")
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

    void FixedUpdate()
    {
        bool doesMove = false;
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal * speed, 0, 0);

        transform.position += movement * Time.deltaTime;

        if (movement.x < 0)
        {
            Flip("left");
            movesQueue.Enqueue("move");
            positionQueue.Enqueue(movement);
            doesMove = true;
}
        else if (movement.x > 0)
        {
            Flip("right");
            movesQueue.Enqueue("move");
            positionQueue.Enqueue(movement);
            doesMove = true;
        }

        if (Input.GetKeyDown("space") && jumping == false)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, forceJump);
            jumping = true;
            movesQueue.Enqueue("jump");
            doesMove = true;
        }

        if (Input.GetMouseButtonDown(0) && attack == false)
        {
            attack = true;
            if (facingRight == true)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<Attack>().cast();
                Invoke("EnableAttack", timeBetweenAttack);
                movesQueue.Enqueue("rightAttack");
                doesMove = true;
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).GetComponent<Attack>().cast();
                Invoke("EnableAttack", timeBetweenAttack);
                movesQueue.Enqueue("leftAttack");
                doesMove = true;
            }
        }
        if (!doesMove)
        {
            movesQueue.Enqueue("idle");
            positionQueue.Enqueue(transform.position);
       }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            jumping = false;
        }
        else
        {
            jumping = true;
        }
    }

    void EnableAttack()
    {
        attack = false;
    }
}
