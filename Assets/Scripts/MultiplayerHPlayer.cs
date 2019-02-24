using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class MultiplayerHPlayer : NetworkBehaviour
{
    public float speed;
    public float timeBetweenAttack;
    public GameObject multiplayerHTwinPrefab;
    public Queue<String> movesQueue = new Queue<String>();
    public Queue<Vector3> positionQueue = new Queue<Vector3>();
    private bool jumping = false;
    private bool attack = false;

    [SyncVar(hook = "OnFacingRightChange")]
    private bool facingRight = true;

    public int timeConstant = 500;
    private int maxHealth = 5;
    private int currentHealth;
    private ArrayList healthGameObjects;
    private float healthTopBarPadding = 1;

    public float damage;

    private Vector3 lastPos;

    public int forceJump;

    private Animator anim;
    private bool setup = false;

    private void Start()
    {

    }

    private void SetupCamera()
    {
        GameObject cam = new GameObject();
        cam.AddComponent<Camera>();
        cam.AddComponent<MultiplayerCameraFollower>();
        var mul = cam.GetComponent<MultiplayerCameraFollower>();

        cam.transform.position = new Vector3(2.25f, 7, -2.73f);
        mul.player = gameObject;
        Instantiate(cam, new Vector3(transform.position.x, transform.position.y, -18), Quaternion.identity);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetupCamera();
        CmdSetupTwin();

        anim = GetComponent<Animator>();

        healthGameObjects = new ArrayList();
        for (int i = 0; i < maxHealth; i++)
        {
            healthGameObjects.Add(AddHealth());
        }
        setup = true;
    }

    [Command]
    void CmdSetupTwin()
    {

        GameObject gO = Instantiate(multiplayerHTwinPrefab, transform.position, transform.rotation);
        MultiplayerHTwin multiplayerHTwin = gO.GetComponent<MultiplayerHTwin>();
        multiplayerHTwin.netId = this.netId;
        multiplayerHTwin.imitateAtTime = Time.time + 3;
        NetworkServer.Spawn(gO);
    }

    GameObject AddHealth()
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
        cube.transform.position = new Vector3(center + (currentHealth - 1) * healthTopBarPadding, spriteHeight + yPos, 0);
        cube.transform.parent = gameObject.transform;
        currentHealth += 1;
        return cube;
    }

    public void takeDamage()
    {
        Destroy((GameObject)healthGameObjects[currentHealth - 1]);
        currentHealth--;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        SceneManager.LoadScene("GameOver");
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

    [Command]
    void CmdFlip(string arg)
    {
        Flip(arg);
    }

    void FixedUpdate()
    {
        if (!setup) return;
        if (!isLocalPlayer) return;

        bool doesMove = false;
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal * speed, 0, 0);

        transform.position += movement * Time.deltaTime;

        if (movement.x < 0)
        {
            anim.SetBool("Running", true);
            Flip("left");
            CmdFlip("left");
            movesQueue.Enqueue("move");
            positionQueue.Enqueue(movement);
            doesMove = true;
        }
        else if (movement.x > 0)
        {
            anim.SetBool("Running", true);
            Flip("right");
            CmdFlip("right");
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
            transform.GetChild(0).GetComponent<MultiplayerAttack>().cast();
            //            Invoke("EnableAttack", timeBetweenAttack);
            movesQueue.Enqueue("rightAttack");
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetComponent<MultiplayerAttack>().cast();
            //            Invoke("EnableAttack", timeBetweenAttack);
            movesQueue.Enqueue("leftAttack");
        }
    }

    public void OnFacingRightChange(bool newFacingRight)
    {
        facingRight = newFacingRight;
        GetComponent<SpriteRenderer>().flipX = !facingRight;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            jumping = false;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            takeDamage();
        }
        else if (other.gameObject.tag == "GroundEnemy")
        {
            takeDamage();
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
        if (!setup) return;
        //        Vector3 currentPos = gameObject.transform.position;
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
        //        else
        //        {
        //            anim.SetBool("Running", false);
        //        }
        //
        //        lastPos = currentPos;
    }
}
