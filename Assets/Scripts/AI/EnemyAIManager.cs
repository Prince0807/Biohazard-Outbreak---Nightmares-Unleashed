using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public Transform[] wayPoints;
    public Transform[] spawnPoints;
    public GameObject[] zombies;

    [SerializeField] private float delayInSpawnningZombies = 2f;

    public Transform zombieRoot;

    private void Start()
    {
        StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {
        GameObject zombie = Instantiate(GetZombie(), zombieRoot, true);
        zombie.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        yield return new WaitForSeconds(delayInSpawnningZombies);
        delayInSpawnningZombies -= 0.001f;
        StartCoroutine(SpawnZombie());
    }

    public GameObject GetZombie()
    {
        return zombies[Random.Range(0,zombies.Length)];
    }
}
