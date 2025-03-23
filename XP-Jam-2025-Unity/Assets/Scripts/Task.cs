using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    [Header("Testing")]
    public bool Test;

    [HideInInspector] public UnityEvent onActivateTask;
    [HideInInspector] public UnityEvent onDecreaseTime;
    [HideInInspector] public UnityEvent onIncreaseTime;
    [HideInInspector] public UnityEvent<Task> onTaskCompleted;
    [HideInInspector] public UnityEvent<Task> onTaskFailed;

    [Header("Refferences")]
    public LightHandler light;
    public TextMeshProUGUI timer;

    [Header("Variables")]

    public int timerIncreaseAmount = 1;

    public int maxTime = 5;
    public int currentMaxTime;
    protected int currentTime = 5;
    protected Coroutine timerCoroutine;

    protected bool locked = true;
    public bool interacted = false;

    public virtual void Start()
    {
        ResetTime();
        light.TurnOff();

        onActivateTask.AddListener(OnActivateTask);
        onIncreaseTime.AddListener(OnIncreaseTimer);
        onTaskCompleted.AddListener(OnTaskCompleted);
        onDecreaseTime.AddListener(OnDecreaseTimer);
        onTaskFailed.AddListener(OnTaskFailed);
    }

    public virtual void Update()
    {
        if (Test && Input.GetKeyDown(KeyCode.B))
        {
            onActivateTask.Invoke();
        }

        if (interacted && Input.GetMouseButtonUp(0))
        {
            interacted = false;
        }


        if (locked) return;
    }

    protected virtual void OnActivateTask()
    {
        locked = false;

        //Turn Light On
        light.TurnOn();

        //Set Timer
        currentTime = currentMaxTime;
        timer.text = currentTime.ToString();
        this.timerCoroutine = StartCoroutine(this.StartTimer());
    }
    protected virtual void OnTaskCompleted(Task task)
    {
        AudioManager.instance.PlayOneShot("Completed");
        locked = true;
        interacted = false;
        light.TurnOff();
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timer.text = "";
    }
    protected virtual void OnTaskFailed(Task task)
    {
        AudioManager.instance.PlayOneShot("Mistake");

        locked = true;

        //Turn Light On
        light.TurnOff();

        //Set Timer
        currentTime = currentMaxTime;
        timer.text = "";
    }
    protected virtual void OnIncreaseTimer()
    {
        currentMaxTime += timerIncreaseAmount;
    }
    protected virtual void OnDecreaseTimer()
    {
        currentMaxTime -= 1;
    }

    public void ResetTime()
    {
        currentMaxTime = maxTime;
    }

    protected IEnumerator StartTimer()
    {
        for (int i = 0; i < currentMaxTime; i++)
        {
            yield return new WaitForSeconds(1);
            currentTime--;

            timer.text = currentTime.ToString();
        }

        if (currentTime <= 0) this.onTaskFailed?.Invoke(this);
    }
}
