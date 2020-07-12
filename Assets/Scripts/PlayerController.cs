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
    private bool canMove = true, isMoving = false, flying = false, flightDistanceSet = false, flightAvailable = true;
    private Vector3 movement = Vector3.zero;
    private Vector3 smoothedPosition, targetPosition;
    private Vector2 facing = Vector2.down;
    private Vector2 flightStartingPoint = Vector2.zero;
    private float currentFlightSpeed = 0f, flightDistance = 0f;
    private MovementManager movementManager;

    // Blood-lust counter
    [Header("Bloodlust Settings")]
    [SerializeField] [Range(1, 10)] private int lustMax = 6;
    [SerializeField] [Range(1, 10)] private int enemyDecreaseAmount = 3;
    private int lustCounter = 0;

    // Collisions
    [Header("Collision Settings")]
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask gateMask;
    [SerializeField] private string boxTag;
    private GameObject touchingBox = null;
    private bool canPushBox = false;
    private Vector2 boxOffset = Vector2.zero;

    // Enemy
    private GameObject enemy = null;

    private void Update()
    {
        // Get movement manager
        if (movementManager == null)
        {
            movementManager = GameObject.FindWithTag("MovementManager").GetComponent<MovementManager>();
        }

        // Collisions
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, wallLayerMask | gateMask);
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, wallLayerMask | gateMask);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, wallLayerMask | gateMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, wallLayerMask | gateMask);

        // Get input (ugly, but necessary for tile-based movement)
        if (!isMoving)
        {
            canPushBox = false;
            if (Input.GetButtonDown("Up"))
            {
                SetInput(upHit, new Vector2(0, 1));
                if (touchingBox != null && !touchingBox.GetComponent<Box>().upHit)
                    canPushBox = true;
            }
            else if (Input.GetButtonDown("Down"))
            {
                SetInput(downHit, new Vector2(0, -1));
                if (touchingBox != null && !touchingBox.GetComponent<Box>().downHit)
                    canPushBox = true;
            }
            else if (Input.GetButtonDown("Right"))
            {
                SetInput(rightHit, new Vector2(1, 0));
                if (touchingBox != null && !touchingBox.GetComponent<Box>().rightHit)
                    canPushBox = true;
            }
            else if (Input.GetButtonDown("Left"))
            {
                SetInput(leftHit, new Vector2(-1, 0));
                if (touchingBox != null && !touchingBox.GetComponent<Box>().leftHit)
                    canPushBox = true;
            }
            else
            {
                movement = Vector3.zero;
            }
        }

        // Look for enemies
        RaycastHit2D enemyLook = Physics2D.Raycast(transform.position, facing, Mathf.Infinity, wallLayerMask | enemyMask);
        if (enemyLook && enemyLook.transform.gameObject.layer == LayerMaskToLayer(enemyMask) && flightAvailable)
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
        if (canMove && !isMoving && movement != Vector3.zero && !flying &&
            (touchingBox == null || (touchingBox != null && canPushBox)))
        {
            isMoving = true;
            movementManager.Move();
            flightAvailable = true;
            targetPosition = transform.position + movement;
            smoothedPosition = transform.position;
        }

        // Move to another space normally
        if (canMove && isMoving && !flying)
        {
            if (Vector3.Distance(smoothedPosition, targetPosition) > 0.1f)
            {
                smoothedPosition = Vector3.Lerp(smoothedPosition, targetPosition, movementSmoothingAmount * Time.deltaTime);
                transform.position = smoothedPosition;

                if (touchingBox != null && canPushBox)
                {
                    boxOffset = targetPosition - smoothedPosition;
                    boxOffset.Normalize();
                    touchingBox.transform.position = transform.position +
                        new Vector3(boxOffset.x, boxOffset.y);
                }
            }
            else
            {
                IncreaseBloodLustCounter(1);
                transform.position = targetPosition;
                movement = Vector3.zero;
                isMoving = false;

                if (touchingBox != null && canPushBox)
                {
                    touchingBox.transform.position = transform.position +
                        new Vector3(boxOffset.x, boxOffset.y);
                }
            }
        }

        // Fly to the enemy if necessary
        if (flying)
        {
            if (Vector2.Distance(transform.position, flightStartingPoint) >= flightDistance)
            {
                transform.position = enemy.transform.position;
                Destroy(enemy);
                DecreaseBloodlustCounter(enemyDecreaseAmount);

                movementManager.Move();
                movement = Vector3.zero;
                flightAvailable = false;
                isMoving = false;
                flying = false;
                flightDistanceSet = false;
            }
            else if (facing.x == 1 && rightHit)
            {
                ResetAfterFlight(rightHit, new Vector3(1, 0));
            }
            else if (facing.x == -1 && leftHit)
            {
                ResetAfterFlight(leftHit, new Vector3(-1, 0));
            }
            else if (facing.y == 1 && upHit)
            {
                ResetAfterFlight(upHit, new Vector3(0, 1));
            }
            else if (facing.y == -1 && downHit)
            {
                ResetAfterFlight(downHit, new Vector3(0, -1));
            }
            else
            {
                isMoving = true;
                currentFlightSpeed += flightAccelerationAmount;
                transform.position += new Vector3(facing.x, facing.y, 0) * currentFlightSpeed;
            }
        }
    }

    public void SetLustMax(int value)
    {
        lustMax = value;
    }

    public int GetLustMax()
    {
        return lustMax;
    }

    public void DecreaseBloodlustCounter(int value)
    {
        lustCounter = Mathf.Max(0, lustCounter - value);
        CheckReload();
    }

    public void IncreaseBloodLustCounter(int value)
    {
        lustCounter = Mathf.Min(lustMax, lustCounter + value);
        CheckReload();
    }

    public bool IsFlying()
    {
        return flying;
    }

    private void SetInput(RaycastHit2D hit, Vector2 direction)
    {
        touchingBox = CheckBoxCollisions(hit);

        if (!hit || touchingBox != null)
        {
            movement = direction;
            facing = direction;
        }
    }

    private GameObject CheckBoxCollisions(RaycastHit2D hit)
    {
        GameObject returnBox = null;
        if (hit && hit.transform.gameObject.tag == boxTag)
            returnBox = hit.transform.gameObject;
        return returnBox;
    }

    private void ResetAfterFlight(RaycastHit2D hit, Vector3 displacement)
    {
        transform.position = hit.transform.position - displacement;
        IncreaseBloodLustCounter(1);

        if (Mathf.Floor(Vector2.Distance(transform.position, flightStartingPoint)) >= Mathf.Floor(flightDistance))
        {
            transform.position = enemy.transform.position;
            Destroy(enemy);
            DecreaseBloodlustCounter(enemyDecreaseAmount);
        }

        movementManager.Move();
        movement = Vector3.zero;
        flightAvailable = false;
        isMoving = false;
        flying = false;
        flightDistanceSet = false;
    }

    private void CheckReload()
    {
        if (lustCounter >= lustMax)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
