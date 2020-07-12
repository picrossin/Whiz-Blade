using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObject sound;

    private GameObject player;
    private bool playedSound = false;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 0.1f)
            {
                if (!playedSound)
                {
                    Instantiate(sound);
                    playedSound = true;
                }
                player.GetComponent<PlayerController>().SetAscending(true);
            }
        }
    }
}
