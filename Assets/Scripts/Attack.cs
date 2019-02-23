﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cast()
    {
        Invoke("destroy", 0.6f);
    }

    void destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print(other);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().takeDamage(100);
        }
    }

}
