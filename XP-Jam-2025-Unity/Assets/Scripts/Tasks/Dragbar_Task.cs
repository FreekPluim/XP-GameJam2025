using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dragbar_Task : Task
{
    [SerializeField] GameObject dial;

    int selectedDial = 0;
    int chosenDial;
    [SerializeField] List<TextMeshProUGUI> spaces = new List<TextMeshProUGUI>();
    bool moved = false;

    Camera cam;

    public override void Start()
    {
        base.Start();
        cam = Camera.main;
    }

    protected override void OnActivateTask()
    {
        base.OnActivateTask();

        chosenDial = Random.Range(0, spaces.Count - 1);
        if (chosenDial >= selectedDial)
        {
            chosenDial++;
        }

        spaces[chosenDial].color = Color.red;
    }

    protected override void OnTaskCompleted(Task task)
    {
        base.OnTaskCompleted(task);

        switch (chosenDial)
        {
            case 0:
                selectedDial = 0;
                dial.transform.localPosition = new Vector3(-0.4f, dial.transform.localPosition.y, dial.transform.localPosition.z);

                break;
            case 1:
                selectedDial = 1;
                dial.transform.localPosition = new Vector3(-0.2f, dial.transform.localPosition.y, dial.transform.localPosition.z);

                break;
            case 2:
                selectedDial = 2;
                dial.transform.localPosition = new Vector3(0f, dial.transform.localPosition.y, dial.transform.localPosition.z);

                break;
            case 3:
                selectedDial = 3;
                dial.transform.localPosition = new Vector3(0.2f, dial.transform.localPosition.y, dial.transform.localPosition.z);

                break;
            case 4:
                selectedDial = 4;
                dial.transform.localPosition = new Vector3(0.4f, dial.transform.localPosition.y, dial.transform.localPosition.z);
                break;

        }
        foreach (var space in spaces)
        {
            space.color = Color.white;
        }
    }

    protected override void OnTaskFailed(Task task)
    {
        base.OnTaskFailed(task);
        foreach (var space in spaces)
        {
            space.color = Color.white;
        }
    }

    RaycastHit hit;
    public override void Update()
    {
        base.Update();

        if (locked) return;

        if (dial.transform.localPosition.x <= 0.4f && dial.transform.localPosition.x >= -0.4f)
        {
            if (interacted && Input.GetMouseButton(0))
            {
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    dial.transform.position = new Vector3(Mathf.Clamp(hit.point.x, -0.4f, 0.4f), dial.transform.position.y, dial.transform.position.z);
                }
                moved = true;
            }
        }

        if (!Input.GetMouseButton(0) && moved)
        {
            AudioManager.instance.PlayOneShot("Dial");
            float xPos = Mathf.Round(dial.transform.localPosition.x * 5) / 5;
            dial.transform.localPosition = new Vector3(Mathf.Clamp(xPos, -0.4f, 0.4f), dial.transform.localPosition.y, dial.transform.localPosition.z);
            moved = false;

            switch (dial.transform.localPosition.x)
            {
                case -0.4f:
                    selectedDial = 0;
                    break;
                case -0.2f:
                    selectedDial = 1;
                    break;
                case 0f:
                    selectedDial = 2;
                    break;
                case 0.2f:
                    selectedDial = 3;
                    break;
                case 0.4f:
                    selectedDial = 4;
                    break;

            }

            if (selectedDial == chosenDial)
            {
                spaces[chosenDial].color = Color.white;
                onTaskCompleted.Invoke(this);
            }

        }
    }

}
