using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("LevelMusic"))
        {
            Destroy(GameObject.FindGameObjectWithTag("LevelMusic"));
        }
    }
}
