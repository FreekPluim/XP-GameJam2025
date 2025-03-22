using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    [Header("Testing")]
    public bool Test;

    [HideInInspector] public UnityEvent onActivateTask;
    [HideInInspector] public UnityEvent onIncreaseTime;
    [HideInInspector] public UnityEvent onTaskCompleted;
    [HideInInspector] public UnityEvent onTaskFailed;

    [Header("Refferences")]
    public LightHandler light;
    public TextMeshProUGUI timer;

    [Header("Variables")]
    public int timerIncreaseAmount = 1;
    public int maxTime = 5;
    protected int currentTime = 5;
    protected Coroutine timerCoroutine;

    protected bool locked = true;
    public bool interacted = false;



    public virtual void Start()
    {
        light.TurnOff();

        onActivateTask.AddListener(OnActivateTask);
        onIncreaseTime.AddListener(OnIncreaseTimer);
        onTaskCompleted.AddListener(OnTaskCompleted);
        onTaskFailed.AddListener(OnTaskFailed);
    }

    public virtual void Update()
    {
        if (Test && Input.GetKeyDown(KeyCode.B))
        {
            onActivateTask.Invoke();
        }

        if (locked) return;
    }

    protected virtual void OnActivateTask()
    {
        locked = false;

        //Turn Light On
        light.TurnOn();

        //Set Timer
        currentTime = maxTime;
        timer.text = currentTime.ToString();
        timerCoroutine = StartCoroutine(StartTimer());
    }
    protected virtual void OnTaskCompleted()
    {
        locked = true;
        interacted = false;
        light.TurnOff();
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timer.text = "";
    }
    protected virtual void OnTaskFailed()
    {
        locked = true;

        //Turn Light On
        light.TurnOff();

        //Set Timer
        currentTime = maxTime;
        timer.text = "";
    }
    protected virtual void OnIncreaseTimer()
    {
        maxTime += timerIncreaseAmount;
    }

    protected IEnumerator StartTimer()
    {
        for (int i = 0; i < maxTime; i++)
        {
            yield return new WaitForSeconds(1);
            currentTime--;

            timer.text = currentTime.ToString();
        }

        if (currentTime <= 0) onTaskFailed.Invoke();
    }
}
