using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBag : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] private int value = 3;

    private GameObject player;

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 0.1f)
            {
                player.GetComponent<PlayerController>().DecreaseBloodlustCounter(value);
                Destroy(gameObject);
            }
        }
    }
}
