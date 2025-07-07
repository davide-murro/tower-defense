using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnDelay = 0f;
    [SerializeField][Range(0.1f, 30f)] float spawnRate = 1f;
    [SerializeField][Range(0, 50)] int poolSize = 5;

    bool isSpawnFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnFinished) CheckEnemiesAlive();
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnDelay);

        for (int i = 0; i < poolSize; i++)
        {
            Instantiate(enemyPrefab, transform, false);
            yield return new WaitForSeconds(spawnRate);
        }

        isSpawnFinished = true;
    }

    // destroy the object when there is no more enemies
    void CheckEnemiesAlive()
    {
        int EnemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (EnemiesCount <= 0) Destroy(gameObject);
    }

}
