using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] [Range(1, 10)] private int lustMax = 6;

    private void Start()
    {
        GameObject instance = Instantiate(player, transform.position, Quaternion.identity);
        instance.GetComponent<PlayerController>().SetLustMax(lustMax);
    }
}
