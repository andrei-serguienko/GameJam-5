using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerAttack : MonoBehaviour
{
    public float damage;

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
        //damage = transform.parent.gameObject.GetComponent<MultiplayerHPlayer>().damage;

        if (other.gameObject == transform.parent.gameObject)
        {
            other.gameObject.GetComponent<MultiplayerHPlayer>().takeDamage();
        }
    }
}
