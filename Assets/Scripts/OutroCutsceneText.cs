using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutroCutsceneText : MonoBehaviour
{
    [SerializeField] private int textSlowness = 2;
    [SerializeField] private List<string> dialogueBoxes = new List<string>();
    [SerializeField] private GameObject sound;

    private int textWaitCount = 0;
    private string currentString = "";
    private int currentDialogueLineIndex = 0;
    private int textCharCount = 0;
    private Text text;
    private bool doneWithLine = false;

    private void Start()
    {
        text = GetComponent<Text>();
        currentString = dialogueBoxes[0];
    }

    private void Update()
    {
        if (textWaitCount == 0 && !doneWithLine)
        {
            if (currentString[textCharCount] != ' ')
            {
                GameObject instance = Instantiate(sound);
                instance.GetComponent<AudioSource>().pitch = Random.Range(0.95f, 1.05f);
            }

            text.text += currentString[textCharCount];
            textCharCount++;

            if (textCharCount == currentString.Length)
            {
                doneWithLine = true;
            }
        }

        textWaitCount++;
        if (textWaitCount > textSlowness)
        {
            textWaitCount = 0;
        }
        

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3") || Input.GetButtonDown("Submit"))
        {            
            textCharCount = 0;
            textWaitCount = 0;
            currentDialogueLineIndex++;
            if (currentDialogueLineIndex < dialogueBoxes.Count)
            {
                text.text = "";
                currentString = dialogueBoxes[currentDialogueLineIndex];
                doneWithLine = false;
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
