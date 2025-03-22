using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject gameplayBox;

    public GameObject ReadyButtonActivate;

    [SerializeField] float textSpeed = 0.03f;
    [SerializeField] float timeBetweenDialogue = 7f;

    [SerializeField] TextMeshProUGUI dialogueText;

    public DialogueSO day1;
    public DialogueSO day2;
    public DialogueSO day3;
    public DialogueSO day4;
    public DialogueSO day5;
    public DialogueSO Filler;

    private void Start()
    {
        StartCoroutine(PlayDialogue(day1));
    }

    public IEnumerator PlayDialogue(DialogueSO day)
    {
        dialogueBox.SetActive(true);
        gameplayBox.SetActive(false);

        for (int i = 0; i < day.dialogue.Count; i++)
        {
            dialogueText.text = "";
            for (int j = 0; j < day.dialogue[i].Length; j++)
            {
                dialogueText.text += day.dialogue[i][j];
                yield return new WaitForSeconds(textSpeed);
            }

            yield return new WaitForSeconds(timeBetweenDialogue);
        }

        //TODO: Activate Button
    }

}
