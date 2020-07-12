using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingGround : MonoBehaviour
{
    [SerializeField] private GameObject pit;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject sound;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            Instantiate(sound);
            Instantiate(pit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
