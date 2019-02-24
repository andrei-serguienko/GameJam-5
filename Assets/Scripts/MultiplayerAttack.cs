using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class MultiplayerAttack : NetworkBehaviour
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("woahh");
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
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Platform" || other.gameObject.tag == "Wall" || other.gameObject.tag == "WallD") {
            return;
        }

        if (gameObject.tag == "Ground" || gameObject.tag == "Platform" || gameObject.tag == "Wall" || gameObject.tag == "WallD")
        {
            return;
        }

        if (other.gameObject.CompareTag("Twin"))
        {
            TakeDamage(other.gameObject);
       }
        else if (other.gameObject.CompareTag("Player") && !other.gameObject.Equals(transform.parent.gameObject)) {
            TakeDamage(other.gameObject);
        }
    }

    void TakeDamage(GameObject gO)
    {
       gO.GetComponent<MultiplayerHPlayer>().takeDamage(); ;
    }
}