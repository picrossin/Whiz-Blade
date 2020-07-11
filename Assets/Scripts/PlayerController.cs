using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement
    [SerializeField] [Range(1.0f, 50.0f)] private float movementSmoothingAmount = 5.0f;
    private bool canMove = true, isMoving = false, flying = false;
    private Vector3 movement = Vector3.zero;
    private Vector3 smoothedPosition, targetPosition;
    private Vector2 facing = Vector2.down;

    // Collisions
    [SerializeField] private LayerMask wallLayerMask, enemyMask;

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
        
        // Movement
        if (canMove && !isMoving && movement != Vector3.zero)
        {
            isMoving = true;
            targetPosition = transform.position + movement;
            smoothedPosition = transform.position;
        }

        if (canMove && isMoving)
        {
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

        // Look for enemies
        RaycastHit2D enemyLook = Physics2D.Raycast(transform.position, facing, Mathf.Infinity, wallLayerMask | enemyMask);
        if (enemyLook && enemyLook.transform.gameObject.layer == layerMaskToLayer(enemyMask))
        {
            flying = true;   
        }
    }

    private int layerMaskToLayer(LayerMask layerMask)
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
