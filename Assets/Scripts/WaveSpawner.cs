 using System.Collections;
 using UnityEngine;
 using UnityEngine.Serialization;
 using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };
   
    [System.Serializable]
    public class Wave
    {
       public string name;
       public Transform[] enemy;
       public int[] count;
       public float rate;
    }

    
    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
    public GameObject[] platforms;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private float searchCountDown = 1f;

    public SpawnState state = SpawnState.COUNTING;

    [FormerlySerializedAs("WaveAnnounce")] public GameObject waveAnnounce;

    private void Start()
    {
        waveCountDown = timeBetweenWaves;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        } else if (state == SpawnState.COUNTING)
        {
            BeforeWave();
        }
        
        
        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
            
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            Debug.LogError("AllWaveComplete");
        }
        else
        {
            AppearPlatform();
            GameObject.FindGameObjectWithTag("Player").GetComponent<HPlayer>().AddHealth(25);
            nextWave++;
        }
        
    }
    
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0)
            return true;
        searchCountDown = 1f;
        return GameObject.FindGameObjectWithTag("Enemy") || GameObject.FindGameObjectWithTag("GroundEnemy") || GameObject.FindGameObjectWithTag("GroundEnemyTwin") || GameObject.FindGameObjectWithTag("GroundEnemyTwin");
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count[0]; ++i)
        {
            SpawnEnemy(wave.enemy[0]);
            yield return new WaitForSeconds(wave.rate);
        }
        for (int i = 0; i < wave.count[1]; ++i)
        {
            SpawnEnemy(wave.enemy[1]);
            yield return new WaitForSeconds(wave.rate);
        }
        for (int i = 0; i < wave.count[2]; ++i)
        {
            SpawnEnemy(wave.enemy[2]);
            yield return new WaitForSeconds(wave.rate);
        }
        for (int i = 0; i < wave.count[3]; ++i)
        {
            SpawnEnemy(wave.enemy[3]);
            yield return new WaitForSeconds(wave.rate);
        }


        state = SpawnState.WAITING;
        
        yield break;
    }

    void SpawnEnemy(Transform enemy)
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Transform create = Instantiate(enemy, sp.transform);
        create.position = sp.position;
    }

    void BeforeWave()
    {
        waveAnnounce.SetActive(true);
        waveAnnounce.GetComponent<Text>().text = RomainText();
        StartCoroutine("Announce");
    }

    private void AppearPlatform()
    {
        platforms[nextWave].SetActive(true);
        for (int i = 0; i < platforms.Length; ++i)
        {
            platforms[i].GetComponent<AudioSource>().Stop();
        }
        platforms[nextWave].GetComponent<AudioSource>().Play();
    }

    private string RomainText()
    {
        switch (nextWave)
        {
            case 0:
                return "WAVE  I";
            case 1:
                return "WAVE  II";
            case 2:
                return "WAVE  III";
            case 3:
                return "WAVE  IV";
            case 4:
                return "WAVE  V";
            case 5:
                return "WAVE  VI";
            case 6:
                return "WAVE  VII";
            case 7:
                return "WAVE  VIII";
            case 8:
                return "WAVE  IX";
            case 9:
                return "WAVE  X";
            default:
                return "WAVE";
        }
    }

    IEnumerator Announce()
    {
        yield return new WaitForSeconds(3f);
        waveAnnounce.SetActive(false);
    }
}

