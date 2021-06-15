using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Propreties
{
    public string name = null;
    public Enemy type = default;
    public float number = 0;
    public float localSpawnTime = 0;
    public float spawnRatio = 0;
    [System.NonSerialized] public bool alreadySpawned = false;
}

[System.Serializable]
public class Wave
{
    public string name = null;
    public float globalTimeStarting = 0;
    public float customDeadTimeRatio = 1;
    [System.NonSerialized] public bool alreadySpawned = false;
    [System.NonSerialized] public float monsterAlive = 0;
    [System.NonSerialized] public bool alreadyReduced = false;
    public List<Propreties> wavelist = null;

    public void Count()
    {
        monsterAlive--;
    }
}
public class EnemyManager : MonoBehaviour
{
    public List<Wave> spawnList = new List<Wave>();
    private float actualTime = 0;
    private ParticleSystem launcher = null;

    IEnumerator Spawner()
    {
        if (spawnList != null)
        {
            int waveIndex = 0;
            int index = 0;
            float startingWaveTime = 0;
            while (waveIndex < spawnList.Count)
            {
                Wave waves = spawnList[waveIndex];

                if (waveIndex > 0 && spawnList[waveIndex-1].monsterAlive == 0 && !waves.alreadyReduced)
                {
                    float temp = (waves.globalTimeStarting - actualTime) / waves.customDeadTimeRatio;
                    waves.globalTimeStarting = actualTime + temp;
                    waves.alreadyReduced = true;
                }

                if (waves.globalTimeStarting <= actualTime && waves.alreadySpawned == false)
                {
                    startingWaveTime = actualTime;

                    while (index < waves.wavelist.Count)
                    {
                        Propreties monsters = waves.wavelist[index];

                        if (monsters.localSpawnTime <= actualTime - startingWaveTime && monsters.alreadySpawned == false)
                        {
                            for (int i = 0; i < monsters.number; i++)
                            {
                                if (launcher != null)
                                {
                                    launcher.Play();
                                }

                                Enemy newEnemy = Instantiate(monsters.type);
                                newEnemy.gameObject.SetActive(true);
                                newEnemy.gameObject.transform.position = transform.position;
                                newEnemy.EventBeforeDeath += waves.Count;
                                waves.monsterAlive++;
                                yield return new WaitForSeconds(monsters.spawnRatio);
                            }

                            monsters.alreadySpawned = true;
                            index++;
                        }

                        else if (monsters.alreadySpawned)
                        {
                            index++;
                        }

                        actualTime = Time.time;
                        yield return null;
                    }

                    index = 0;
                    waves.alreadySpawned = true;
                    waveIndex++;
                }

                else if (waves.alreadySpawned)
                {
                    waveIndex++;
                }

                actualTime = Time.time;
                yield return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        actualTime = Time.time;
        foreach (Wave wave in spawnList)
        {
            wave.globalTimeStarting += Time.time;
        }
        launcher = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(Spawner());
    }

    public bool isAllEnemiesDead()
    {
        foreach (Wave wave in spawnList)
        {
            if (!wave.alreadySpawned || wave.alreadySpawned && wave.monsterAlive > 0)
            {
                return false;
            }
        }
        return true;
    }
}
