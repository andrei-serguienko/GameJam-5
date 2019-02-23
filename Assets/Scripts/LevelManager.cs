using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    
    public GameObject title;
    public GameObject FlyingEnemy;
    
    public int currentWave;
    // Start is called before the first frame update
    void Start()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        Title();

        Instantiate(FlyingEnemy);

    }

    private void Title()
    {
        title.SetActive(true);
        title.GetComponent<Text>().text = "Wave " + currentWave;
        Invoke("DisapearTitle", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisapearTitle()
    {
        title.SetActive(false);
    }
}
