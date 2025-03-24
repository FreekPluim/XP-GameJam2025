using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject gameplayBox;

    public GameObject ReadyButtonActivate;

    public UnityEvent endOfDialoguePlay;

    [SerializeField] float textSpeed = 0.03f;
    [SerializeField] float timeBetweenDialogue = 7f;

    [SerializeField] TextMeshProUGUI dialogueText;

    public List<DialogueSO> dialogueDays;

    public void PlayDialogue(int day)
    {
        StartCoroutine(PlayDialogueCo(day));
    }

    public void PlayDialogueSpecific(DialogueSO dialogue)
    {
        StartCoroutine(PlayDialogueSpecificCo(dialogue));
    }

    public void PlayDialogueText(string text)
    {
        StartCoroutine(PlayDialogueTextCo(text));
    }

    public IEnumerator PlayDialogueCo(int day)
    {
        dialogueBox.SetActive(true);
        gameplayBox.SetActive(false);

        if (day < dialogueDays.Count)
        {
            for (int i = 0; i < dialogueDays[day].dialogue.Count; i++)
            {
                dialogueText.text = "";
                for (int j = 0; j < dialogueDays[day].dialogue[i].Length; j++)
                {
                    dialogueText.text += dialogueDays[day].dialogue[i][j];
                    AudioManager.instance.PlayOneShot("Text");
                    yield return new WaitForSeconds(textSpeed);
                }

                yield return new WaitForSeconds(timeBetweenDialogue);
            }
        }
        else
        {
            for (int i = 0; i < dialogueDays[dialogueDays.Count - 1].dialogue.Count; i++)
            {
                dialogueText.text = "";
                for (int j = 0; j < dialogueDays[dialogueDays.Count - 1].dialogue[i].Length; j++)
                {
                    dialogueText.text += dialogueDays[dialogueDays.Count - 1].dialogue[i][j];
                    AudioManager.instance.PlayOneShot("Text");
                    yield return new WaitForSeconds(textSpeed);

                }

                yield return new WaitForSeconds(timeBetweenDialogue);
            }
        }

        endOfDialoguePlay.Invoke();
    }
    public IEnumerator PlayDialogueSpecificCo(DialogueSO dialogue)
    {
        dialogueBox.SetActive(true);
        gameplayBox.SetActive(false);

        for (int i = 0; i < dialogue.dialogue.Count; i++)
        {
            dialogueText.text = "";
            for (int j = 0; j < dialogue.dialogue[i].Length; j++)
            {
                dialogueText.text += dialogue.dialogue[i][j];
                AudioManager.instance.PlayOneShot("Text");
                yield return new WaitForSeconds(textSpeed);
            }

            yield return new WaitForSeconds(timeBetweenDialogue);
        }

        endOfDialoguePlay?.Invoke();
    }
    public IEnumerator PlayDialogueTextCo(string text)
    {
        dialogueBox.SetActive(true);
        gameplayBox.SetActive(false);

        dialogueText.text = "";
        for (int j = 0; j < text.Length; j++)
        {
            dialogueText.text += text[j];
            AudioManager.instance.PlayOneShot("Text");
            yield return new WaitForSeconds(textSpeed);
        }

        endOfDialoguePlay?.Invoke();
    }
}
