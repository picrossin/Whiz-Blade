using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteOnPlay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
