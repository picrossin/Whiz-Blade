using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pit : MonoBehaviour
{
    [SerializeField] private GameObject sound;

    private GameObject player;
    private bool filled = false, soundPlayed = false;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 0.1f && !filled && !player.GetComponent<PlayerController>().IsFlying())
            {
                if (!soundPlayed)
                {
                    Instantiate(sound);
                    soundPlayed = true;
                }
                player.GetComponent<PlayerController>().SetFalling(true);
            }
        }
    }

    public void SetFilledTexture(Sprite filledSprite)
    {
        GetComponent<SpriteRenderer>().sprite = filledSprite;
        filled = true;
    }
}
