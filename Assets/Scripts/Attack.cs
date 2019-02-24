﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;

    public GameObject particleEffect;
    // Start is called before the first frame update
    void Start()
    {
        cast();
//        OnTriggerEnter2D(ga);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void cast()
    {
        Invoke("destroy", 0.3f);
    }

    void destroy()
    {
//        gameObject.SetActive(false);
        Destroy(gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        damage = GameObject.FindWithTag("Player").GetComponent<HPlayer>().damage;

        print(other);
        if (other.gameObject.tag == "Enemy")
        {    
            other.gameObject.GetComponent<Enemy>().takeDamage(50);
//            print((gameObject.transform.position - other.gameObject.transform.position) * 5);
//            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-(gameObject.transform.position - other.gameObject.transform.position) * 5, ForceMode2D.Impulse);      
            
        } else if (other.gameObject.tag == "GroundEnemy")
        {
            other.gameObject.GetComponent<GroundEnemy>().takeDamage(damage);
        }
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HPlayer>().takeDamage(20);
        }
        else if (other.gameObject.tag == "GroundEnemyTwin" && gameObject.tag == "Twin")
        {
            other.gameObject.GetComponent<GroundEnemy>().takeDamage(damage);
        }
        else if (other.gameObject.tag == "EnemyTwin" && gameObject.tag == "Twin")
        {
            other.gameObject.GetComponent<Enemy>().takeDamage(damage);
        }
        
        
    }

    

    private void OnDisable()
    {
        
    }

    private void OnEnable()
    {
//        Instantiate(particleEffect, tr);
        
//gameObject.GetComponent<ParticleSystem>().Emit(100);    
    }
}
