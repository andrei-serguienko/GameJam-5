using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   
    public float speed;
    public float timeBetweenAttack;
    private bool jumping = false;
    private bool attack = false;
    private bool facingRight = true;

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
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");

        Vector3 movement = new Vector3 (moveHorizontal * speed, 0, 0);

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
    
    void Update () {
        if (Input.GetKeyDown("space") && jumping == false)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, forceJump);
            jumping = true;
        }

        print(attack);
        if (Input.GetMouseButtonDown(0) && attack == false)
        {
            attack = true;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Attack>().cast();
            Invoke("EnableAttack", timeBetweenAttack);
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
