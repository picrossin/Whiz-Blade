using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private string movingEnemyTag = "MovingEnemy";
    private List<GameObject> movingEnemies = new List<GameObject>();

    private void Start()
    {
        movingEnemies = GameObject.FindGameObjectsWithTag(movingEnemyTag).ToList();
    }

    private void Update()
    {
        List<GameObject> movingEnemiesUpdate = GameObject.FindGameObjectsWithTag(movingEnemyTag).ToList();
        if (movingEnemiesUpdate.Except(movingEnemies).Count() > 0 ||
            movingEnemies.Except(movingEnemiesUpdate).Count() > 0)
        {
            movingEnemies = movingEnemiesUpdate;
        }
    }

    public void Move()
    {
        foreach (GameObject movingEnemy in movingEnemies)
        {
            movingEnemy.GetComponent<MovingEnemy>().Move();
        }
    }
}
