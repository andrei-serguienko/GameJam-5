using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;

    public int enemySpeed;

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
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
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
    }

    void checkAlive()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
