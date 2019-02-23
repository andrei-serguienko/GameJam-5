using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {    
            other.gameObject.GetComponent<Enemy>().takeDamage(50);
//            print((gameObject.transform.position - other.gameObject.transform.position) * 5);
//            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-(gameObject.transform.position - other.gameObject.transform.position) * 5, ForceMode2D.Impulse);

            
            
        } else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HPlayer>().takeDamage();
        }
    }

}
