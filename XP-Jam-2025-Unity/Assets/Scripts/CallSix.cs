using System.Collections.Generic;
using UnityEngine;

public class CallSix : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask mask;

    [SerializeField] List<DialogueSO> calls;
    public GameObject blur;

    [SerializeField] Material mat;
    [SerializeField] Color active, inactive;

    public bool canCall = false;

    private void Start()
    {
        cam = Camera.main;
        mat.color = inactive;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canCall)
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, mask))
            {
                AudioManager.instance.PlayOneShot("Click");
                GameManager.instance.softReset();

                StartCoroutine(GameManager.instance.dialogueManager.PlayDialogueSpecific(calls[GameManager.instance.calledSixAmount]));
                GameManager.instance.calledSixAmount++;
                canCall = false;

                switch (GameManager.instance.calledSixAmount)
                {
                    case 1:
                        GameManager.instance.dialogueManager.endOfDialoguePlay.AddListener(FirstCall);
                        break;
                    case 2:
                        GameManager.instance.dialogueManager.endOfDialoguePlay.AddListener(SecondCall);
                        break;
                    case 3:
                        GameManager.instance.dialogueManager.endOfDialoguePlay.AddListener(ThirdCall);
                        break;
                    case 4:
                        GameManager.instance.dialogueManager.endOfDialoguePlay?.AddListener(GameManager.instance.OnSacrificedEnding);
                        blur.SetActive(false);
                        break;
                    default:
                        break;
                }

                if (GameManager.instance.calledSixAmount < 4)
                {
                    GameManager.instance.dialogueManager.endOfDialoguePlay.AddListener(GameManager.instance.readyButton.ActivateButton);
                    GameManager.instance.onReadyPressed.AddListener(GameManager.instance.OnReadyPressed);
                }
            }
        }
    }

    public void Activate()
    {
        canCall = true;
        mat.color = active;
    }

    public void Deactivate()
    {
        canCall = false;
        mat.color = inactive;
    }

    void FirstCall()
    {
        GameManager.instance.dialogueManager.endOfDialoguePlay.RemoveListener(FirstCall);
        foreach (var task in GameManager.instance.tasks)
        {
            task.onIncreaseTime?.Invoke();
        }
    }

    void SecondCall()
    {
        GameManager.instance.dialogueManager.endOfDialoguePlay.RemoveListener(SecondCall);

        foreach (var task in GameManager.instance.tasks)
        {
            task.onIncreaseTime?.Invoke();
        }

        //Remove sound from left side
    }

    void ThirdCall()
    {
        GameManager.instance.dialogueManager.endOfDialoguePlay.RemoveListener(ThirdCall);

        foreach (var task in GameManager.instance.tasks)
        {
            task.onIncreaseTime?.Invoke();
        }

        //Create blurry vision for right side of screen.
        blur.SetActive(true);
    }
}
