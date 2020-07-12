using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private bool shaking = false;
    private Vector3 initialPosition = Vector3.zero;
    private float intensity = 0f;
    private int frequency = 1, freqCounter = 0;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (shaking)
        {
            if (freqCounter == 0)
            {
                transform.position += new Vector3(Random.Range(intensity / 3f, intensity),
                    Random.Range(intensity / 3f, intensity), 0);
            }
            else
            {
                transform.position = initialPosition;
            }

            freqCounter++;
            if (freqCounter > frequency)
            {
                freqCounter = 0;
            }
        }
        else
        {
            transform.position = initialPosition;
        }
    }

    public void ShakeScreen(float shakeIntensity, float length, int freq = 1)
    {
        frequency = freq;
        freqCounter = 0;
        StartCoroutine(ShakeForSeconds(length));
        intensity = shakeIntensity;
        shaking = true;
    }

    private IEnumerator ShakeForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        shaking = false;
    }
}
