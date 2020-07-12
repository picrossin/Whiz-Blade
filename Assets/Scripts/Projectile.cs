using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] [Range(1.0f, 50.0f)] private float movementSmoothingAmount = 40.0f;
    [SerializeField] private string[] collisionTags;
    [SerializeField] private GameObject sprite;

    private bool moving;
    private Vector3 smoothedPosition = Vector3.zero, targetPosition = Vector3.zero;
    private GameObject player;
    private bool overBox = false;
    private GameObject currentBox;

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
                player.GetComponent<PlayerController>().IncreaseBloodLustCounter(player.GetComponent<PlayerController>().GetLustMax());
                Destroy(gameObject);
            }
        }

        if (overBox && Vector2.Distance(transform.position, currentBox.transform.position) < 0.1f)
        {
            overBox = false;
            Destroy(gameObject);
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
        if (dir.x == 1)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (dir.y == 1)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (dir.y == -1)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in collisionTags)
        {
            if (collision.tag == "Box")
            {
                overBox = true;
                currentBox = collision.gameObject;
            }
            if (collision.tag == tag)
            {
                Destroy(gameObject);
            }
        }
    }
}
