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
    public int timeTwin = 3;

    private Animator anim;

    public HPlayer player;

    public GameObject LeftAttackT;
    public GameObject RightAttackT;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
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
            anim.SetBool("Running", true);
        }
        else if (movement.x > 0)
        {
            Flip("right");
            anim.SetBool("Running", true);
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
        Instantiate(LeftAttackT, transform);
        Invoke("EnableAttack", timeBetweenAttack);
        anim.SetTrigger("attack");

    }

    void RightAttack()
    {
        Instantiate(RightAttackT, transform);
        Invoke("EnableAttack", timeBetweenAttack);
        anim.SetTrigger("attack");
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
                    LeftAttack();
                    break;
                case "rightAttack":
                    RightAttack();
                    break;
                case "idle":
                    transform.position = player.positionQueue.Dequeue();
                    anim.SetBool("Running", false);
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

    private void Update()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y > 1)
        {
            anim.SetBool("isJumping", true);
        } else
        {
            anim.SetBool("isJumping", false);
        }
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y < -0.5)
        {
            anim.SetBool("isFalling", true);
        } else
        {
            anim.SetBool("isFalling", false);
        }
    }

    public void Attack()
    {
        
    }
    
}
