using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillScript : MonoBehaviour
{
    public Image redFlag;
    bool filling = true;
    public float waitTime = 0.5f;

    private void Start()
    {
        redFlag.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (filling == true)
        {
            //Reduce fill amount over 30 seconds
            redFlag.fillAmount += 1.0f / waitTime *Time.deltaTime;
            Debug.Log(redFlag.fillAmount);
        }
    }
}
