 using System.Collections;
 //using UnityEditor.PackageManager;
 using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    
    public enum SpawnState { SPAWNING, WAITING, COUNTING }
    
    [System.Serializable]
    public class Wave
    {
       public string name;
       public Transform enemy;
       public int count;
       public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private float searchCountDown = 1f;

    public SpawnState state = SpawnState.COUNTING;

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
            nextWave++;
        }
        
    }
    
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0)
            return true;
        searchCountDown = 1f;
        return GameObject.FindGameObjectWithTag("Enemy");
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; ++i)
        {
            SpawnEnemy(wave.enemy);
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
}

