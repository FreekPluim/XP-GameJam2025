using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotateDial_Task : Task
{
    Camera cam;

    [SerializeField] GameObject pivot;
    [SerializeField] List<TextMeshProUGUI> positions;
    [SerializeField] GameObject handle;

    int chosenPos;
    int selectedPos = 0;

    public override void Start()
    {
        base.Start();

        cam = Camera.main;
    }

    protected override void OnActivateTask()
    {
        base.OnActivateTask();

        chosenPos = Random.Range(0, positions.Count - 1);
        if (chosenPos >= selectedPos)
        {
            chosenPos++;
        }

        positions[chosenPos].color = Color.red;
    }

    protected override void OnTaskCompleted(Task task)
    {
        base.OnTaskCompleted(task);

        foreach (var pos in positions)
        {
            pos.color = Color.white;
        }
    }
    protected override void OnTaskFailed(Task task)
    {
        base.OnTaskFailed(task);

        foreach (var pos in positions)
        {
            pos.color = Color.white;
        }
        selectedPos = chosenPos;

        switch (chosenPos)
        {
            case 0:
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, -45, pivot.transform.localRotation.z);
                break;
            case 1:
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, 45, pivot.transform.localRotation.z);
                break;
            case 2:
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, -135, pivot.transform.localRotation.z);
                break;
            case 3:
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, 135, pivot.transform.localRotation.z);
                break;
        }
    }

    RaycastHit hit;
    bool moved;
    Vector3 targetPostition;
    public override void Update()
    {
        base.Update();

        if (locked) return;

        if (interacted && Input.GetMouseButton(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                targetPostition = new Vector3(hit.point.x, pivot.transform.position.y, hit.point.z);
                pivot.transform.localRotation = Quaternion.LookRotation(targetPostition - pivot.transform.position, Vector3.up);
                moved = true;
            }
        }
        else if (!Input.GetMouseButton(0) && moved)
        {
            SetAngle();
            moved = false;

            if (selectedPos == chosenPos)
            {
                onTaskCompleted?.Invoke(this);
            }
        }
    }

    float GetAngle()
    {
        return Vector3.Angle(pivot.transform.forward, Vector3.forward);
    }

    void SetAngle()
    {
        if (handle.transform.position.x > pivot.transform.position.x)
        {
            if (GetAngle() < 90)
            {
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, 45, pivot.transform.localRotation.z);
                selectedPos = 1;
                AudioManager.instance.PlayOneShot("Dial");
            }
            else
            {
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, 135, pivot.transform.localRotation.z);
                selectedPos = 3;
                AudioManager.instance.PlayOneShot("Dial");
            }
        }
        else
        {
            if (GetAngle() < 90)
            {
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, -45, pivot.transform.localRotation.z);
                selectedPos = 0;
                AudioManager.instance.PlayOneShot("Dial");
            }
            else
            {
                pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, -135, pivot.transform.localRotation.z);
                selectedPos = 2;
                AudioManager.instance.PlayOneShot("Dial");
            }
        }
    }
}
