using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private bool shootOnFirstMove = false;
    [SerializeField] private Vector2 direction = Vector2.zero;

    private bool shoot = false;

    private void Start()
    {
        shoot = shootOnFirstMove;
    }

    public void Move()
    {
        if (shoot)
        {
            GameObject instance = Instantiate(projectile,
                transform.position + new Vector3(direction.x, direction.y, transform.position.z),
                Quaternion.identity);

            instance.GetComponent<Projectile>().SetDirection(direction);
            shoot = false;
        }
        else
        {
            shoot = true;
        }
    }
}
