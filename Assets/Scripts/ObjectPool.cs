using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnDelay = 0f;
    [SerializeField][Range(0.1f, 30f)] float spawnRate = 1f;
    [SerializeField][Range(0, 50)] int poolSize = 5;


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

        for (int i = 0; i < poolSize; i++)
        {
            Instantiate(enemyPrefab, transform, false);
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
