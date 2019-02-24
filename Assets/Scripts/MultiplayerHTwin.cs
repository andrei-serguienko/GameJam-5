using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;


public class MultiplayerHTwin : NetworkBehaviour
{
    private float speed = 10;
    private float timeBetweenAttack = 0;
    private bool jumping = false;
    private bool attack = false;

    [SyncVar(hook = "OnFacingRightChange")]
    private bool facingRight = true;

    private int forceJump = 25;
    private int timeTwin = 3;

    private Animator anim;

    public MultiplayerHPlayer player;
    public float imitateAtTime;


    private void Start()
    {
        anim = GetComponent<Animator>();
        //player = NetworkServer.FindLocalObject(netId).GetComponent<MultiplayerHPlayer>();
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

    [Command]
    void CmdFlip(string arg)
    {
        Flip(arg);
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
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<MultiplayerAttack>().cast();
        Invoke("EnableAttack", timeBetweenAttack);
        anim.SetTrigger("attack");

    }

    void RightAttack()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<MultiplayerAttack>().cast();
        Invoke("EnableAttack", timeBetweenAttack);
        anim.SetTrigger("attack");
    }

    public void OnFacingRightChange(bool newFacingRight)
    {
        facingRight = newFacingRight;
        GetComponent<SpriteRenderer>().flipX = !facingRight;
    }


    void FixedUpdate()
    {
        if (player == null) return;
        if (Time.time > imitateAtTime && player.movesQueue.Count > 0)
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
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y < -0.5)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }
}
