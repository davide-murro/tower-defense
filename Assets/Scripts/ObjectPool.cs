using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnDelay = 0f;
    [SerializeField][Range(0.1f, 30f)] float spawnRate = 1f;
    [SerializeField][Range(0, 50)] int poolSize = 5;

    GameObject[] pool;

    void Awake()
    {
        PopulatePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnDelay);
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i].SetActive(true);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            enemyPrefab.SetActive(false);
            pool[i] = Instantiate(enemyPrefab, transform, false);
        }
    }

}
