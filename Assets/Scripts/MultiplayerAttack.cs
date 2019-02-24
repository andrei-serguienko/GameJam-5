using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        //damage = transform.parent.gameObject.GetComponent<MultiplayerHPlayer>().damage;

        if (other.gameObject == transform.parent.gameObject)
        {
            CmdTakeDamage(other.gameObject.GetComponent<MultiplayerHPlayer>());
        }
    }

    [Command]
    void CmdTakeDamage(MultiplayerHPlayer hp)
    {
        hp.takeDamage();
    }
}