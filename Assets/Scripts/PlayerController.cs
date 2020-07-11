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
    [SerializeField] [Range(1, 10)] private int enemyDecreaseAmount = 3;
    private int lustCounter = 0;

    // Collisions
    [Header("Collision Settings")]
    [SerializeField] private LayerMask wallLayerMask, enemyMask;
    [SerializeField] private string boxTag;
    private GameObject touchingBox = null;
    private bool canPushBox = false;
    private Vector2 boxOffset = Vector2.zero;

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
        if (canMove && !isMoving && movement != Vector3.zero && !flying && 
            (touchingBox == null || (touchingBox != null && canPushBox)))
        {
            isMoving = true;
            targetPosition = transform.position + movement;
            smoothedPosition = transform.position;
            IncreaseBloodLustCounter(1);
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
