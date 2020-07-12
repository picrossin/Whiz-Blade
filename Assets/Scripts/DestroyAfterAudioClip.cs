using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAudioClip : MonoBehaviour
{
    private void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
