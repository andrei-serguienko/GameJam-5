using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject[] players;
    private Animator anim;

    public int enemySpeed;

    public int health = 200;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("Attack", 3f);
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Attack()
    {
        if (players.Length == 0) return; 
        GameObject player = players[0];
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

    public void takeDamage(int dmg)
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
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-other.relativeVelocity * 300, ForceMode2D.Impulse);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Invoke("Attack", 3f);
        }
    }
}
