using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pit : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] private int value = 6;

    private GameObject player;
    private bool filled = false;

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
                player.GetComponent<PlayerController>().IncreaseBloodLustCounter(value);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void SetFilledTexture(Sprite filledSprite)
    {
        GetComponent<SpriteRenderer>().sprite = filledSprite;
        filled = true;
    }
}
