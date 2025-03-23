using UnityEngine;

public class Lever_Task : Task
{
    public float rotationMultiplier = 5;
    public GameObject LeverPivot;
    public GameObject LeverHead;
    bool up = true;
    public float angle = 50;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (!locked)
        {
            if (Input.GetMouseButton(0) && interacted)
            {
                if (Vector3.Angle(LeverPivot.transform.up, Vector3.up) <= angle)
                {
                    LeverPivot.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotationMultiplier, 0, 0));
                }

                if (Vector3.Angle(LeverPivot.transform.up, Vector3.up) > angle)
                {

                    //Check dir
                    if (LeverHead.transform.position.z > LeverPivot.transform.position.z)
                    {
                        //Up Reached
                        LeverPivot.transform.rotation = Quaternion.Euler(angle - 0.1f, 0, 0);
                        if (!up)
                        {
                            onTaskCompleted?.Invoke(this);
                        }

                    }
                    if (LeverHead.transform.position.z < LeverPivot.transform.position.z)
                    {
                        //Down Reached
                        LeverPivot.transform.rotation = Quaternion.Euler(-angle + 0.1f, 0, 0);
                        if (up)
                        {
                            onTaskCompleted?.Invoke(this);
                        }
                    }
                }
            }
            else
            {
                interacted = false;
            }
        }
    }

    protected override void OnTaskCompleted(Task task)
    {
        AudioManager.instance.PlayOneShot("Lever");
        base.OnTaskCompleted(task);

        if (up)
        {
            LeverPivot.transform.rotation = Quaternion.Euler(-angle + 0.1f, 0, 0);
        }
        else
        {
            LeverPivot.transform.rotation = Quaternion.Euler(angle - 0.1f, 0, 0);
        }
        up = !up;

    }
}
