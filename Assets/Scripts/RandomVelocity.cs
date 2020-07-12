using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVelocity : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private float rotationSpeed = 0f;
    private Vector2 velocity = Vector2.zero;

    private void Start()
    {
        rotationSpeed = Random.Range(1.0f, 5.0f);
        velocity = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized * speed;
    }

    private void Update()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(0.001f, 0.001f, 0);
        }
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + rotationSpeed);
        transform.position += new Vector3(velocity.x, velocity.y, 0);
    }
}
