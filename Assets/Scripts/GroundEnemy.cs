using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    private bool Played = false;
    private GameObject player;
    private Animator anim;

    public int enemySpeed;

    public float health = 100;

    public bool touching = false;

    public AudioClip[] MyAudio;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("Attack", 3f);
        player = GameObject.FindGameObjectWithTag("Player");
              
    }

    void Attack()
    {
       
        float direction = player.transform.position.x - gameObject.transform.position.x;
        if (direction > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(enemySpeed, 0, 0);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-enemySpeed, 0, 0);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        checkAlive();
        if (Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x) > 0.5f)
        {
            anim.SetBool("Moving", true);
            if (!Played)
            {
                print("SS");
                GetComponent<AudioSource>().Play();
                Played = true;
            }
        }
        else
        {
            anim.SetBool("Moving", false);
            if (Played)
            {
                GetComponent<AudioSource>().Stop();
                Played = false;
            }
            if(touching)
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 25));
        }
    }

    public void takeDamage(float dmg)
    {
//        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Invoke("Attack", 2f);
        GetComponent<AudioSource>().PlayOneShot(MyAudio[2]);
        anim.SetTrigger("Hit");
        health -= dmg;
    }

    void checkAlive()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetTrigger("Attacking");
            GetComponent<AudioSource>().PlayOneShot(MyAudio[1]);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-other.relativeVelocity.x * 300, 0), ForceMode2D.Impulse);
//            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Invoke("Attack", 3f);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        touching = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        touching = false;
        Attack();
    }
}
