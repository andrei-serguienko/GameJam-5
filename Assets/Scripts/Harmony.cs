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
            float difference = Mathf.Abs(Vector3.Distance(player.transform.position, twin.transform.position));
            difference = Mathf.Exp(difference);
            if (difference > 100)
            {
                difference = 100;
            }

            float inRange = 40 - ((difference / 100) * 40 + 1);
            if (inRange < 1)
                inRange = 1;
            else if (inRange > 40)
                inRange = 40;

            modFrequency = Mathf.Round(inRange);
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
