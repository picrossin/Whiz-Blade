using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Collisions
    [Header("Collision Settings")]
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private string pitTag = "Pit";
    [SerializeField] private Sprite pitFilledSprite;

    public bool upHit = false, downHit = false, rightHit = false, leftHit = false;
    private bool overPit = false, overEnemy;
    private GameObject pitObject, enemyObject;

    private void Update()
    {
        // Collisions
        upHit = Physics2D.Raycast(transform.position + new Vector3(0, 0.6f), Vector2.up, 0.4f, wallLayerMask);
        downHit = Physics2D.Raycast(transform.position + new Vector3(0, -0.6f), Vector2.down, 0.4f, wallLayerMask);
        rightHit = Physics2D.Raycast(transform.position + new Vector3(0.6f, 0), Vector2.right, 0.4f, wallLayerMask);
        leftHit = Physics2D.Raycast(transform.position + new Vector3(-0.6f, 0), Vector2.left, 0.4f, wallLayerMask);

        if (overPit && Vector2.Distance(transform.position, pitObject.transform.position) < 0.1f)
        {
            pitObject.GetComponent<Pit>().SetFilledTexture(pitFilledSprite);
            Destroy(gameObject);
            overPit = false;
        }
        if (overEnemy && Vector2.Distance(transform.position, enemyObject.transform.position) < 0.1f)
        {
            Destroy(enemyObject);
            overEnemy = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == pitTag)
        {
            pitObject = collision.gameObject;
            overPit = true;
        } 
        else if (collision.tag == "Enemy" || collision.tag == "MovingEnemy" || collision.tag == "ShootingEnemy")
        {
            enemyObject = collision.gameObject;
            overEnemy = true;
        }
    }
}
