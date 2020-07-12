using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBag : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] private int value = 3;
    [SerializeField] private string healthBarTag = "HealthBar";
    [SerializeField] private GameObject slurpSound;

    private GameObject player, healthBar;

    void Update()
    {
        if (healthBar == null)
        {
            healthBar = GameObject.FindWithTag(healthBarTag);
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 0.2f)
            {
                if (healthBar != null)
                {
                    healthBar.GetComponent<HealthBar>().PlayJuiceAnimation();
                }
                player.GetComponent<PlayerController>().DecreaseBloodlustCounter(value);

                Instantiate(slurpSound);

                Destroy(gameObject);
            }
        }
    }
}
