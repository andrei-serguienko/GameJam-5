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
    private int health = 5;
    private ArrayList healthGameObjects;
    private float healthTopBarPadding = 1;

    private Vector3 lastPos;

    public int forceJump;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
        healthGameObjects = new ArrayList();
        for(int i=0;i<health;i++)
        {
            float spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
            float spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
            float yPos = transform.position.y - 1;
            float xPos = transform.position.x;
            float center = xPos - spriteWidth / 2 - 0.5f;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.GetComponent<Renderer>().material.color = Color.red;
            Vector3 theScale = cube.transform.localScale;
            theScale.x *= 0.5f;
            theScale.y *= 0.5f;
            cube.transform.localScale = theScale;
            cube.transform.position = new Vector3(center + i * healthTopBarPadding, spriteHeight + yPos, 0);
            cube.transform.parent = gameObject.transform;
            healthGameObjects.Add(cube);
        }
        
    }
    
    public void takeDamage(int dmg)
    {
        print("AIE !");
        for(int i = 0; i <= dmg; ++i)
            Destroy((GameObject)healthGameObjects[healthGameObjects.Count - 1]);
    }

    public void regenHealth(int qt)
    {
        //Create a cube health
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

    void FixedUpdate()
    {
        bool doesMove = false;
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal * speed, 0, 0);

        transform.position += movement * Time.deltaTime;

        if (movement.x < 0)
        {
            anim.SetBool("Running", true);
            Flip("left");
            movesQueue.Enqueue("move");
            positionQueue.Enqueue(movement);
            doesMove = true;
}
        else if (movement.x > 0)
        {
            anim.SetBool("Running", true);
            Flip("right");
            movesQueue.Enqueue("move");
            positionQueue.Enqueue(movement);
            doesMove = true;
        }
        else
        {
            anim.SetBool("Running", false);
        }

        if (Input.GetKeyDown("w") && jumping == false)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, forceJump);
            movesQueue.Enqueue("jump");
            jumping = true;
            doesMove = true;
        }

        if (Input.GetKeyDown("space"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("FirstAttack"))
            {
                anim.SetBool("SecondAttack", true);
            }
            else
            {
                anim.SetTrigger("attack");
            }
            attack = true;
            
        }
        if (!doesMove)
        {
            movesQueue.Enqueue("idle");
            positionQueue.Enqueue(transform.position);
       }
    }

    public void Attack()
    {
        if (facingRight == true)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Attack>().cast();
//            Invoke("EnableAttack", timeBetweenAttack);
            movesQueue.Enqueue("rightAttack");
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetComponent<Attack>().cast();
//            Invoke("EnableAttack", timeBetweenAttack);
            movesQueue.Enqueue("leftAttack");
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
//        Vector3 currentPos = gameObject.transform.position;
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
//        else
//        {
//            anim.SetBool("Running", false);
//        }
//
//        lastPos = currentPos;
    }
}
