using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenInput : MonoBehaviour
{
    [SerializeField] private Animator fade;

    private bool started = false;

    private void Update()
    {
        if (!started)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3") || Input.GetButtonDown("Submit"))
            {
                started = true;
                fade.SetTrigger("SwipeIn");
                StartCoroutine(WaitForTransition());
            }
        }
    }

    private IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
