public class Button_Task : Task
{
    public override void Update()
    {
        base.Update();

        if (!locked && interacted)
        {
            AudioManager.instance.PlayOneShot("Click");
            onTaskCompleted.Invoke(this);

        }
    }
}
