using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Collisions
    [Header("Collision Settings")]
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private string pitTag = "Pit";

    public bool upHit = false, downHit = false, rightHit = false, leftHit = false;

    private void Update()
    {
        // Collisions
        upHit = Physics2D.Raycast(transform.position + new Vector3(0, 0.6f), Vector2.up, 0.4f, wallLayerMask);
        downHit = Physics2D.Raycast(transform.position + new Vector3(0, -0.6f), Vector2.down, 0.4f, wallLayerMask);
        rightHit = Physics2D.Raycast(transform.position + new Vector3(0.6f, 0), Vector2.right, 0.4f, wallLayerMask);
        leftHit = Physics2D.Raycast(transform.position + new Vector3(-0.6f, 0), Vector2.left, 0.4f, wallLayerMask);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == pitTag)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
