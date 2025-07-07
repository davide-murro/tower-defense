using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;


    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindFirstObjectByType<GridManager>();
        pathFinder = GetComponentInParent<PathFinder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    /*
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
    */

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            // when i move in the Y axis i have to divide it in 2 movements
            if (endPosition.y > startPosition.y)
            {
                Vector3 verticalEndPosition = new Vector3(startPosition.x + ((endPosition.x - startPosition.x) / 2), endPosition.y, startPosition.z + ((endPosition.z - startPosition.z) / 2));
                yield return StartCoroutine(FollowMovement(startPosition, verticalEndPosition));
            }
            else if (endPosition.y < startPosition.y)
            {
                Vector3 horizontalEndPosition = new Vector3(startPosition.x + ((endPosition.x - startPosition.x) / 2), startPosition.y, startPosition.z + ((endPosition.z - startPosition.z) / 2));
                yield return StartCoroutine(FollowMovement(startPosition, horizontalEndPosition));
            }

            // the walk the rest
            Vector3 currentStartPosition = transform.position;
            yield return StartCoroutine(FollowMovement(currentStartPosition, endPosition));
        }

        FinishPath();
    }

    IEnumerator FollowMovement(Vector3 startPosition, Vector3 endPosition)
    {
        float travelPercent = 0f;

        transform.LookAt(endPosition);

        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
            yield return new WaitForEndOfFrame();
        }
    }

    public void RecalculatePath(bool resetPath)
    {
        // check the start
        Vector3Int coordinates = new Vector3Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
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
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
