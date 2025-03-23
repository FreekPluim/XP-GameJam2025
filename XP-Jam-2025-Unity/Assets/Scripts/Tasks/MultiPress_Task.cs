using TMPro;
using UnityEngine;

public class MultiPress_Task : Task
{
    int pressCount;
    public TextMeshProUGUI number;

    protected override void OnActivateTask()
    {
        base.OnActivateTask();
        pressCount = Random.Range(4, 7);
        number.text = pressCount.ToString();
    }
    protected override void OnTaskCompleted(Task task)
    {
        base.OnTaskCompleted(task);
        number.text = "";
    }
    protected override void OnTaskFailed(Task task)
    {
        base.OnTaskFailed(task);
        number.text = "";
    }
    public override void Update()
    {
        base.Update();

        if (interacted && !locked && Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlayOneShot("Click");
            pressCount--;
            number.text = pressCount.ToString();

            if (pressCount <= 0)
            {
                onTaskCompleted?.Invoke(this);
            }
        }
    }
}
