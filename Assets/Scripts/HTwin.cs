using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HTwin : MonoBehaviour
{
    private float speed;
    private float timeBetweenAttack;
    private bool jumping = false;
    private bool attack = false;
    private bool facingRight = true;
    private int forceJump;
    private int timeTwin = 3;

    public HPlayer player;

    private void Start()
    {
        speed = player.speed;
        timeBetweenAttack = player.timeBetweenAttack;
        forceJump = player.forceJump;
    }

    void Flip(string arg)
    {
        if (arg == "left")
        {
            if (facingRight)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                facingRight = false;
            }
        }
        else if (arg == "right")
        {
            if (!facingRight)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                facingRight = true;
            }
        }
    }

    void Move(Vector3 movement)
    {
        transform.position += movement * Time.deltaTime;
        if (movement.x < 0)
        {
            Flip("left");
        }
        else if (movement.x > 0)
        {
            Flip("right");
        }
    }

    void Jump()
    {
        if (jumping == false)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, forceJump);
            jumping = true;
        }
    }

    void LeftAttack()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Attack>().cast();
        Invoke("EnableAttack", timeBetweenAttack);
    }

    void RightAttack()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<Attack>().cast();
        Invoke("EnableAttack", timeBetweenAttack);
    }

    void FixedUpdate()
    {

        if (Time.time > timeTwin)
            switch (player.movesQueue.Dequeue())
            {
                case "move":
                    Move(player.positionQueue.Dequeue());
                    break;
                case "jump":
                    Jump();
                    break;
                case "leftAttack":
                    print("At");
                    LeftAttack();
                    break;
                case "rightAttack":
                    RightAttack();
                    break;
                case "idle":
                    transform.position = player.positionQueue.Dequeue();
                    break;
                default:
                    break;
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
