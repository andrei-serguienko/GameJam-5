using UnityEngine;
using System.Collections;

public class Harmony : MonoBehaviour
{
    [Range(1, 20000)]  //Creates a slider in the inspector
    public float frequency1;

    [Range(1, 20000)]  //Creates a slider in the inspector
    public float frequency2;

    [Range(1, 40)]
    public float modFrequency;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;
    private bool changed = false;

    private float difference;

    AudioSource audioSource;
    int timeIndex = 0;

    private GameObject player;
    private GameObject twin;

    private int index = 0;

    void Start()
    {
        frequency1 = 100;
        frequency2 = 100;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        timeIndex = 0;  //resets timer before playing sound
        audioSource.Play();

        player = GameObject.FindWithTag("Player");
        twin = GameObject.FindWithTag("Twin");
    }

    void Update()
{
    GameObject.FindWithTag("Harmony").transform.localScale = new Vector3(difference / 100 * 4 + 1, difference / 100 * 4 + 1, difference / 100 * 4 + 1);
    GameObject.FindWithTag("Player").GetComponent<HPlayer>().damage = difference;

    audioSource.volume = (1f-(difference/100f))/3f;
    
        if (changed)
        {
            if (index < 30)
            {
                index += 1;
            }
            else
            {
                changed = false;
                index = 0;
            }
        }
        else
        {
            difference = Mathf.Abs(Vector3.Distance(player.transform.position, twin.transform.position));
//            difference = Mathf.Sqrt(difference);
//            if (difference > 100)
//            {
//                difference = 100;
//            }
            difference = Mathf.Abs(difference / 30) * 100;
            difference = 100- difference;

//            float inRange = 40 - ((difference / 100) * 40 + 1);
            if (difference < 1)
                difference = 1;
            else if (difference > 100)
                difference = 100;

            modFrequency = 4;
            changed = true;

        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate) *
                      CreateSine(timeIndex, modFrequency, sampleRate) ;

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate) *
                                CreateSine(timeIndex, modFrequency, sampleRate);

            timeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }
}
