using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
//    private int maxHealth = 5;
    private int currentHealth = 100;
    private int currentArmor = 20;
        
    public GameObject UiHealth;
    public GameObject UiArmor;
//    private ArrayList healthGameObjects;
//    private float healthTopBarPadding = 1;

    private bool enableMove = true;
    private bool wallRight = false;
    
    public float damage;

    private bool enableToJumpWall = false;

    private Vector3 lastPos;

    public int forceJump;

    private Animator anim;

    public GameObject HitUI;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    void UpdateUi()
    {
        float transHealth = (float) currentHealth;
        UiHealth.transform.localScale = new Vector3(transHealth/100, 1, 1);
        float transArmor = (float) currentArmor;
        UiArmor.transform.localScale = new Vector3(transArmor/100, 1, 1);
    }

    public void AddHealth(int qt)
    {
        print("ADD");
        if (currentArmor >= 100)
        {
            return;
        }
        if (currentHealth >= 100)
        {
            print("ED");
            currentArmor += qt;
            if (currentArmor > 100)
                currentArmor = 100;
            return;
        }
        currentHealth += qt;
        if (currentHealth > 100)
        {
            int goToArmor = currentHealth - 100;
            currentArmor += goToArmor;
            currentHealth = 100;
            if (currentArmor > 100)
                currentArmor = 100;
        }
    }
    
    public void takeDamage(int qt)
    {
        HitUI.SetActive(true);
        if (currentArmor > 0)
        {
            currentArmor -= qt;
            if (currentArmor < 0)
            {
                currentHealth += currentArmor;
                currentArmor = 0;
                
            }
            return;   
        }
        else
        {
            print("TAKE");
            currentHealth -= qt;
            
        }

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        SceneManager.LoadScene("GameOver");
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
        Vector3 movement = Vector3.one;


        if (!enableMove)
        {
//            movement = new Vector3(moveHorizontal * speed, 0, 0);
        }
        else
        {
            movement = new Vector3(moveHorizontal * speed, 0, 0);
        }

         
        
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
            if (enableToJumpWall)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (wallRight)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce( new Vector2(-forceJump* 5000, forceJump*8000));
                    enableMove = false;
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce( new Vector2(forceJump* 5000, forceJump*8000));
                    enableMove = false;
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, forceJump*8000));
            }
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
        enableMove = true;
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Platform")
        {
            jumping = false;
            enableToJumpWall = false;
            enableMove = true;
            
        } else if (other.gameObject.tag == "Wall")
        {
            wallRight = false;
            enableToJumpWall = true;
            jumping = false; 
        } else if (other.gameObject.tag == "WallD")
        {
            wallRight = true;
            enableToJumpWall = true;
            jumping = false; 
        }else if (other.gameObject.tag == "Enemy")
        {
            takeDamage(20);
        } else if (other.gameObject.tag == "GroundEnemy")
        {
            takeDamage(20);
        }
        else
        {
            jumping = true;
        }
        
    }

//    private void OnCollisionExit2D(Collision2D other)
//    {
//        if (other.gameObject.tag == "Wall")
//        {
//            enableMove = false;
//        } else if (other.gameObject.tag == "WallD")
//        {
//            enableMove = false;
//
//        }
//    }

//    void EnableAttack()
//    {
//        attack = false;
//    }

    private void Update()
    {
        UpdateUi();
        print(currentHealth);
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
