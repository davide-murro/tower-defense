using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    public List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;


    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindFirstObjectByType<GridManager>();
        pathFinder = FindFirstObjectByType<PathFinder>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
    public void RecalculatePath(bool resetPath)
    {
        // check the start
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        } else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        // reset path
        //StopCoroutine(FollowPath()); // it doesn t work!!
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        enemy.PenalizeGold();
        gameObject.SetActive(false);
    }

}
