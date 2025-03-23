using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public UnityEvent onDayStarted;
    [HideInInspector] public UnityEvent onReadyPressed;
    [HideInInspector] public UnityEvent onDayEndedGood;
    [HideInInspector] public UnityEvent onDayEndedBad;
    [HideInInspector] public UnityEvent onReset;

    public DialogueManager dialogueManager;

    public GameObject dialogueBox;
    public GameObject gameplayBox;

    public List<Task> tasks;

    public int day = 4;
    int mistakes = 0;
    int majorMistakes = 0;
    public int dayTimerMax = 30;
    int currentTime;

    public Color gray;

    [Header("UI")]
    public TextMeshProUGUI gameTime;
    public TextMeshProUGUI dayUI;
    public List<TextMeshProUGUI> mistakesUI;

    [Header("Ready Button")]
    public ReadyButton readyButton;

    [Header("Call Button")]
    [SerializeField] CallSix callSixButton;

    [Header("Dialogue")]
    [SerializeField] DialogueSO perfectDay;
    [SerializeField] DialogueSO badDay;
    [SerializeField] DialogueSO badDayTwo;
    [SerializeField] DialogueSO killedEnding;
    [SerializeField] DialogueSO firedEnding;

    [SerializeField] GameObject pentagram, blood;

    Coroutine timer;
    Coroutine dayProgress;

    public int calledSixAmount = 0;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);

        onDayStarted.AddListener(OnDayStarted);
        onDayEndedGood.AddListener(OnDayEndNoMistakes);
        onDayEndedBad.AddListener(OnDayEndThreeMistakes);
        onReset.AddListener(ResetValues);

        OnQuitPressed();
    }
    void ResetValues()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].gameObject.SetActive(false);
            tasks[i].ResetTime();
            tasks[i].onTaskFailed.RemoveListener(OnMistakeMade);
        }

        pentagram.SetActive(false);
        blood.SetActive(false);

        minMaxTaskTime = new Vector2(3.5f, 5.5f);
        gameTime.text = "0:" + currentTime.ToString();
        calledSixAmount = 0;
        mistakes = 0;
        majorMistakes = 0;
        day = 0;
        currentTime = dayTimerMax;
    }

    #region Day Started
    void OnDayStarted()
    {
        StartCoroutine(dialogueManager.PlayDialogue(day - 1));

        readyButton.text.text = "READY";
        onReadyPressed.AddListener(OnReadyPressed);
        dialogueManager.endOfDialoguePlay.AddListener(readyButton.ActivateButton);

    }
    #endregion
    #region Ready Pressed
    public void OnReadyPressed()
    {
        mistakes = 0;
        dialogueBox.SetActive(false);
        gameplayBox.SetActive(true);
        onReadyPressed.RemoveListener(OnReadyPressed);

        timer = StartCoroutine(Timer());
        dayProgress = StartCoroutine(ActivateRandomTasks());

        callSixButton.Activate();
    }
    #endregion
    #region No Mistakes End Of Day
    void OnDayEndNoMistakes()
    {
        callSixButton.Deactivate();
        StopCoroutine(dayProgress);
        CompleteAllActiveTasks();

        //Play dialogue for day end
        StartCoroutine(dialogueManager.PlayDialogueSpecific(perfectDay));
        dialogueManager.endOfDialoguePlay.AddListener(EndOfDayCallack);
    }
    #endregion
    #region Three Mistakes End of Day
    void OnDayEndThreeMistakes()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].onTaskFailed.RemoveListener(OnMistakeMade);
        }
        callSixButton.Deactivate();
        StopCoroutine(dayProgress);
        StopCoroutine(timer);
        CompleteAllActiveTasks();

        majorMistakes++;

        if (majorMistakes == 3)
        {
            //GAME OVER
            if (calledSixAmount > 0)
            {
                StartCoroutine(dialogueManager.PlayDialogueSpecific(killedEnding));
                dialogueManager.endOfDialoguePlay.AddListener(OnKilledEnding);
            }
            else
            {
                StartCoroutine(dialogueManager.PlayDialogueSpecific(firedEnding));
                dialogueManager.endOfDialoguePlay.AddListener(OnFiredEnding);
            }
        }
        else
        {
            //Play dialogue for bad day end
            if (majorMistakes == 1)
            {
                StartCoroutine(dialogueManager.PlayDialogueSpecific(badDay));
            }
            if (majorMistakes == 2)
            {
                StartCoroutine(dialogueManager.PlayDialogueSpecific(badDayTwo));
            }
            dialogueManager.endOfDialoguePlay.AddListener(EndOfDayCallack);
        }
    }
    #endregion
    public void CompleteAllActiveTasks()
    {
        foreach (var task in tasks)
        {
            task.onTaskCompleted.Invoke(task);
        }
    }
    void ToNextDay()
    {
        onReadyPressed.RemoveAllListeners();

        mistakes = 0;
        currentTime = dayTimerMax;
        gameTime.text = "0:" + currentTime.ToString();
        day++;
        dayUI.text = day.ToString();

        if (allInactiveTasks != null) allInactiveTasks.Clear();
        //Deactivate all
        if (day < 5)
        {
            minMaxTaskTime.x -= 0.25f;
            minMaxTaskTime.y -= 0.25f;
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].gameObject.SetActive(false);
            }

            //Activate ones used for this round
            if (day < 4)
            {
                for (int i = 0; i < day * 2; i++)
                {
                    allInactiveTasks.Add(tasks[i]);
                    tasks[i].gameObject.SetActive(true);
                }
            }
            else if (day == 4)
            {
                for (int i = 0; i < 7; i++)
                {
                    allInactiveTasks.Add(tasks[i]);
                    tasks[i].gameObject.SetActive(true);
                }
            }
        }
        else if (day == 5)
        {

            for (int i = 0; i < 7; i++)
            {
                tasks[i].gameObject.SetActive(true);
                tasks[i].onDecreaseTime.Invoke();
                allInactiveTasks.Add(tasks[i]);
            }
        }
        else
        {
            minMaxTaskTime.x -= 0.15f;
            minMaxTaskTime.y -= 0.15f;
            for (int i = 0; i < 7; i++)
            {
                allInactiveTasks.Add(tasks[i]);
            }
        }

        for (int i = 0; i < mistakesUI.Count; i++)
        {
            mistakesUI[i].color = gray;
        }

        onDayStarted?.Invoke();
    }
    public void OnMistakeMade(Task task)
    {
        Debug.Log("Called by " + task.name);
        mistakes++;
        task.onTaskFailed.RemoveListener(OnMistakeMade);

        if (mistakes == 3)
        {
            onDayEndedBad?.Invoke();
            return;
        }
        if (mistakesUI.Count > mistakes) mistakesUI[mistakes - 1].color = Color.red;

        allInactiveTasks.Add(task);
    }
    void OnTaskCompleted(Task task)
    {
        task.onTaskCompleted.RemoveListener(OnTaskCompleted);
        task.onTaskFailed.RemoveListener(OnMistakeMade);
        allInactiveTasks.Add(task);
    }
    IEnumerator Timer()
    {
        currentTime = dayTimerMax;

        for (int i = 0; i < dayTimerMax; i++)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            gameTime.text = "0:" + currentTime.ToString();
        }

        onDayEndedGood.Invoke();
    }
    List<Task> allInactiveTasks = new List<Task>();

    Vector2 minMaxTaskTime = new Vector2(3.5f, 5.5f);
    IEnumerator ActivateRandomTasks()
    {
        for (int i = 0; i < 100; i++)
        {
            if (allInactiveTasks.Count > 0)
            {
                for (int j = 0; j < Random.Range(1, allInactiveTasks.Count + 1); j++)
                {
                    int randomTask = Random.Range(0, allInactiveTasks.Count);
                    Task task = allInactiveTasks[randomTask];
                    allInactiveTasks.RemoveAt(randomTask);

                    task.onTaskFailed.AddListener(OnMistakeMade);
                    task.onTaskCompleted.AddListener(OnTaskCompleted);

                    task.onActivateTask.Invoke();
                }
            }

            yield return new WaitForSeconds(Mathf.Clamp(Random.Range(minMaxTaskTime.x, minMaxTaskTime.y), 1, 6));
        }
    }
    public void softReset()
    {
        CompleteAllActiveTasks();

        StopCoroutine(timer);
        StopCoroutine(dayProgress);

        currentTime = dayTimerMax;
        gameTime.text = "0:" + currentTime.ToString();

    }

    #region Endings Callbacks
    void OnKilledEnding()
    {
        //Play gun sound
        AudioManager.instance.PlayOneShot("Gun");
        blood.SetActive(true);

        dialogueManager.endOfDialoguePlay.RemoveListener(OnKilledEnding);

        //Go to end screen
        causeOfDeath = "Killed";

        Statistics();

    }
    void OnFiredEnding()
    {
        dialogueManager.endOfDialoguePlay.RemoveListener(OnFiredEnding);

        causeOfDeath = "Fired";

        Statistics();

    }
    public void OnSacrificedEnding()
    {
        pentagram.SetActive(true);
        dialogueManager.endOfDialoguePlay.RemoveListener(OnSacrificedEnding);

        causeOfDeath = "Sacrificed";

        Statistics();
    }
    #endregion

    #region end of day callbacks
    void EndOfDayCallack()
    {
        //Fade to black.

        //Load next day
        ToNextDay();

        //Fade in

        dialogueManager.endOfDialoguePlay.RemoveListener(EndOfDayCallack);
    }
    #endregion
    string causeOfDeath = "";
    void Statistics()
    {
        callSixButton.blur.SetActive(false);
        StartCoroutine(dialogueManager.PlayDialogueText(
            "You were " + causeOfDeath + "\n \n" +
            "You survived until day : " + day.ToString()));

        readyButton.text.text = "QUIT";
        dialogueManager.endOfDialoguePlay.AddListener(readyButton.ActivateButton);
        onReadyPressed.AddListener(OnQuitPressed);
    }
    void OnQuitPressed()
    {
        onReadyPressed.RemoveAllListeners();

        onReset.Invoke();
        StartCoroutine(dialogueManager.PlayDialogueText("Welcome to:\n\n" + "CALL FOR ACTION! \n\n\n" + "By Singular Salt Grain"));
        readyButton.text.text = "START";
        dialogueManager.endOfDialoguePlay.AddListener(readyButton.ActivateButton);
        onReadyPressed.AddListener(ToNextDay);
    }
}
