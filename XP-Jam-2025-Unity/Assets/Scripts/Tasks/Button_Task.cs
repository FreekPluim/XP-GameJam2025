public class Button_Task : Task
{
    public override void Update()
    {
        base.Update();

        if (!locked && interacted)
        {
            onTaskCompleted.Invoke();
        }
    }
}
