using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 50.0f)] private float movementSmoothingAmount = 20.0f;
    [SerializeField] private GameObject enemyPortrait, playerPortrait;
    [SerializeField] private Sprite happyFace, hungryFace, reallyHungryFace, deadFace, juiceFace;
    [SerializeField] private Sprite playerHappyFace, playerSadFace, playerDeadFace;

    private float targetHealth = 0f, smoothedHealth = 0f;
    private Slider slider;
    private GameObject player;
    private bool playingJuice = false, juiceStarted = false;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            int currentPlayerHealth = player.GetComponent<PlayerController>().GetCurrentLust();
            slider.maxValue = player.GetComponent<PlayerController>().GetLustMax();
            smoothedHealth = Mathf.Lerp(smoothedHealth, currentPlayerHealth,
                movementSmoothingAmount * Time.deltaTime);
            slider.value = smoothedHealth;

            if (playingJuice)
            {
                if (!juiceStarted)
                {
                    juiceStarted = true;
                    StartCoroutine(JuiceAnimation());
                }
            }
            else
            {
                if (currentPlayerHealth == 0)
                {
                    enemyPortrait.GetComponent<SpriteRenderer>().sprite = happyFace;
                    playerPortrait.GetComponent<SpriteRenderer>().sprite = playerHappyFace;
                }
                else if (currentPlayerHealth < player.GetComponent<PlayerController>().GetLustMax() / 2)
                {
                    enemyPortrait.GetComponent<SpriteRenderer>().sprite = hungryFace;
                    playerPortrait.GetComponent<SpriteRenderer>().sprite = playerSadFace;
                }
                else if (currentPlayerHealth >= player.GetComponent<PlayerController>().GetLustMax() / 2)
                {
                    enemyPortrait.GetComponent<SpriteRenderer>().sprite = reallyHungryFace;
                    playerPortrait.GetComponent<SpriteRenderer>().sprite = playerSadFace;
                }
                else if (currentPlayerHealth == player.GetComponent<PlayerController>().GetLustMax())
                {
                    enemyPortrait.GetComponent<SpriteRenderer>().sprite = deadFace;
                    playerPortrait.GetComponent<SpriteRenderer>().sprite = playerDeadFace;
                }
            }
        }
    }

    public void PlayJuiceAnimation()
    {
        playingJuice = true;
    }

    private IEnumerator JuiceAnimation()
    {
        enemyPortrait.GetComponent<SpriteRenderer>().sprite = juiceFace;
        yield return new WaitForSeconds(1f);
        juiceStarted = false;
        playingJuice = false;
    }
}
