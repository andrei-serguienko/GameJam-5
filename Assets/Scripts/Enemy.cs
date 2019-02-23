using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Animator anim;

    public int enemySpeed;

    public float health = 100;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("Attack", 3f);
        player = GameObject.FindGameObjectWithTag("Player");
              
    }

    void Attack()
    {
        
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 100);
        
        gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * enemySpeed;  
    }
    // Update is called once per frame
    void Update()
    {
        checkAlive();

        if (Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.x) > 0 || Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().velocity.y) > 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }

    public void takeDamage(float dmg)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Invoke("Attack", 2f);
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
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-other.relativeVelocity.x * 300, 0), ForceMode2D.Impulse);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().rotation = 0;
            Invoke("Attack", 3f);
        } else if (other.gameObject.tag == "Ground")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().rotation = 0;
            Invoke("Attack", 3f);
        }
    }
}
