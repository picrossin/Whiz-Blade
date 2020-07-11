using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class EnemyPath : MonoBehaviour
{
    [SerializeField] private List<GameObject> pathPoints = new List<GameObject>();

    private void Update()
    {
        List<GameObject> pathPointsUpdated = new List<GameObject>();

        foreach (Transform child in transform)
        {
            pathPointsUpdated.Add(child.gameObject);
        }

        if (pathPointsUpdated.Except(pathPoints).Count() > 0 ||
            pathPoints.Except(pathPointsUpdated).Count() > 0)
        {
            pathPoints = pathPointsUpdated;
        }
    }

    public List<GameObject> GetPathPoints()
    {
        return pathPoints;
    }
}
