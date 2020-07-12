using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private string movingEnemyTag = "MovingEnemy";
    [SerializeField] private string shootingEnemyTag = "ShootingEnemy";
    [SerializeField] private string projectileTag = "Projectile";
    [SerializeField] private GameObject levelMusic;

    private List<GameObject> movingEnemies = new List<GameObject>();
    private List<GameObject> shootingEnemies = new List<GameObject>();
    private List<GameObject> projectiles = new List<GameObject>();

    private void Start()
    {
        movingEnemies = GameObject.FindGameObjectsWithTag(movingEnemyTag).ToList();
        shootingEnemies = GameObject.FindGameObjectsWithTag(shootingEnemyTag).ToList();
        projectiles = GameObject.FindGameObjectsWithTag(projectileTag).ToList();

        if (!GameObject.FindGameObjectWithTag("LevelMusic"))
        {
            Instantiate(levelMusic, Vector3.zero, Quaternion.identity);
        }
    }

    private void Update()
    {
        List<GameObject> movingEnemiesUpdate = GameObject.FindGameObjectsWithTag(movingEnemyTag).ToList();
        if (movingEnemiesUpdate.Except(movingEnemies).Count() > 0 ||
            movingEnemies.Except(movingEnemiesUpdate).Count() > 0)
        {
            movingEnemies = movingEnemiesUpdate;
        }

        List<GameObject> shootingEnemiesUpdate = GameObject.FindGameObjectsWithTag(shootingEnemyTag).ToList();
        if (shootingEnemiesUpdate.Except(shootingEnemies).Count() > 0 ||
            shootingEnemies.Except(shootingEnemiesUpdate).Count() > 0)
        {
            shootingEnemies = shootingEnemiesUpdate;
        }

        List<GameObject> projectilesUpdate = GameObject.FindGameObjectsWithTag(projectileTag).ToList();
        if (projectilesUpdate.Except(projectiles).Count() > 0 ||
            projectiles.Except(projectilesUpdate).Count() > 0)
        {
            projectiles = projectilesUpdate;
        }
    }

    public void Move()
    {
        foreach (GameObject movingEnemy in movingEnemies)
        {
            movingEnemy.GetComponent<MovingEnemy>().Move();
        }

        foreach (GameObject shootingEnemy in shootingEnemies)
        {
            shootingEnemy.GetComponent<ShootingEnemy>().Move();
        }

        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().Move();
        }
    }
}
