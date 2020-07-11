using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement Settings")]
    [SerializeField] [Range(1.0f, 50.0f)] private float movementSmoothingAmount = 5.0f;
    [SerializeField] [Range(0.001f, 1.0f)] private float baseFlySpeed = 0.15f;
    [SerializeField] [Range(0.001f, 1.0f)] private float flightAccelerationAmount = 0.1f;
    private bool canMove = true, isMoving = false, flying = false, flightDistanceSet = false;
    private Vector3 movement = Vector3.zero;
    private Vector3 smoothedPosition, targetPosition;
    private Vector2 facing = Vector2.down;
    private Vector2 flightStartingPoint = Vector2.zero;
    private float currentFlightSpeed = 0f, flightDistance = 0f;

    // Blood-lust counter
    [Header("Bloodlust Settings")]
    [SerializeField] [Range(1, 10)] private int lustMax = 6;
    private int lustCounter = 0;

    // Collisions
    [Header("Collision Settings")]
    [SerializeField] private LayerMask wallLayerMask, enemyMask;

    // Enemy
    private GameObject enemy = null;

    private void Update()
    {
        // Collisions
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, 1, wallLayerMask);
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, 1, wallLayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 1, wallLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 1, wallLayerMask);

        // Get input (ugly, but necessary for tile-based movement)
        if (!isMoving)
        {
            if (Input.GetButtonDown("Up") && !upHit)
            {
                movement = new Vector3(0, 1);
                facing = new Vector2(0, 1);
            }
            else if (Input.GetButtonDown("Down") && !downHit)
            {
                movement = new Vector3(0, -1);
                facing = new Vector2(0, -1);
            }
            else if (Input.GetButtonDown("Right") && !rightHit)
            {
                movement = new Vector3(1, 0);
                facing = new Vector2(1, 0);
            }
            else if (Input.GetButtonDown("Left") && !leftHit)
            {
                movement = new Vector3(-1, 0);
                facing = new Vector2(-1, 0);
            }
            else
            {
                movement = Vector3.zero;
            }
        }

        // Look for enemies
        RaycastHit2D enemyLook = Physics2D.Raycast(transform.position, facing, Mathf.Infinity, wallLayerMask | enemyMask);
        if (enemyLook && enemyLook.transform.gameObject.layer == LayerMaskToLayer(enemyMask))
        {
            enemy = enemyLook.transform.gameObject;
            flying = true;

            if (!flightDistanceSet)
            {
                flightDistance = Vector3.Distance(transform.position, enemy.transform.position);
                flightStartingPoint = transform.position;
                flightDistanceSet = true;
            }
        }

        // Check for movement
        if (canMove && !isMoving && movement != Vector3.zero && !flying)
        {
            isMoving = true;
            targetPosition = transform.position + movement;
            smoothedPosition = transform.position;
            lustCounter++;
        }

        // Move to another space normally
        if (canMove && isMoving && !flying)
        {
            Debug.Log("Regular move");
            if (Vector3.Distance(smoothedPosition, targetPosition) > 0.1f)
            {
                smoothedPosition = Vector3.Lerp(smoothedPosition, targetPosition, movementSmoothingAmount * Time.deltaTime);
                transform.position = smoothedPosition;
            }
            else
            {
                transform.position = targetPosition;
                movement = Vector3.zero;
                isMoving = false;
            }
        }

        // Fly to the enemy if necessary
        if (flying)
        {
            if (Vector2.Distance(transform.position, flightStartingPoint) >= flightDistance)
            {
                transform.position = enemy.transform.position;
                Destroy(enemy);
                isMoving = false;
                flying = false;
                flightDistanceSet = true;
            }
            else
            {
                isMoving = true;
                currentFlightSpeed += flightAccelerationAmount;
                transform.position += new Vector3(facing.x, facing.y, 0) * currentFlightSpeed;
            }
        }

        if (lustCounter >= lustMax)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void DecreaseBloodlustCounter(int value)
    {
        lustCounter = Mathf.Max(0, lustCounter - value);
    }

    public void IncreaseBloodLustCounter(int value)
    {
        lustCounter = Mathf.Min(lustMax, lustCounter + value);
    }

    public bool IsFlying()
    {
        return flying;
    }

    private int LayerMaskToLayer(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;

        while (layer > 0)
        {
            layer >>= 1;
            layerNumber++;
        }

        return layerNumber - 1;
    }
}
