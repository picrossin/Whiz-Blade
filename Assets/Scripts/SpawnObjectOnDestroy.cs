using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    private void OnDestroy()
    {
        Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
    }
}
