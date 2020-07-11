using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementSmoothingAmount = 40.0f;
    [SerializeField] [Range(1, 6)] private int damage = 3;
    [SerializeField] private string[] collisionTags;

    private bool moving;
    private Vector3 smoothedPosition = Vector3.zero, targetPosition = Vector3.zero;
    private GameObject player;

    private void Start()
    {
        targetPosition = transform.position;
        smoothedPosition = targetPosition;
    }

    private void Update()
    {
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
                transform.position = targetPosition;
                moving = false;
            }
        }
        else
        {
            if (player != null && Vector2.Distance(player.transform.position, transform.position) < 0.1f)
            {
                player.GetComponent<PlayerController>().IncreaseBloodLustCounter(damage);
                Destroy(gameObject);
            }
        }
    }

    public void Move()
    {
        targetPosition = transform.position + new Vector3(direction.x, direction.y, 0);
        moving = true;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in collisionTags)
        {
            if (collision.tag == tag)
            {
                Destroy(gameObject);
            }
        }
    }
}
