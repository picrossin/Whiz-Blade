using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    [SerializeField] private EnemyPath enemyPath;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementSmoothingAmount = 20.0f;
    [SerializeField] private LayerMask wallLayerMask;

    private GameObject currentPoint;
    private Vector3 smoothedPosition = Vector3.zero, targetPosition = Vector3.zero;
    private int pointIndex = 0;
    private bool moving = false;
    private GameObject player;

    private void Start()
    {
        currentPoint = enemyPath.GetPathPoints()[0];
        targetPosition = transform.position;
        smoothedPosition = targetPosition;
    }

    private void Update()
    {
        // Get player
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        // Smooth movement that runs every frame
        if (moving)
        {
            if (Vector3.Distance(smoothedPosition, targetPosition) > 0.1f)
            {
                smoothedPosition = Vector3.Lerp(smoothedPosition, targetPosition, movementSmoothingAmount * Time.deltaTime);
                transform.position = smoothedPosition;
            }
            else
            {
                if (player != null && Vector2.Distance(player.transform.position, transform.position) < 0.1f)
                {
                    player.GetComponent<PlayerController>().IncreaseBloodLustCounter(player.GetComponent<PlayerController>().GetLustMax());
                }

                transform.position = targetPosition;
                moving = false;
            }
        }
    }

    public void Move()
    {
        pointIndex++;
        if (pointIndex >= enemyPath.GetPathPoints().Count)
        {
            pointIndex = 0;
        }

        currentPoint = enemyPath.GetPathPoints()[pointIndex];
        targetPosition = new Vector3(currentPoint.transform.position.x,
            currentPoint.transform.position.y,
            transform.position.z);

        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            (targetPosition - transform.position).normalized,
            1, wallLayerMask);

        if (hit)
        {
            if (pointIndex == 0)
            {
                pointIndex = enemyPath.GetPathPoints().Count - 1;
            }
            else
            {
                pointIndex--;
            }

            currentPoint = enemyPath.GetPathPoints()[pointIndex];
            targetPosition = transform.position;
            moving = false;
        }
        else
        {
            moving = true;
        }
    }
}
