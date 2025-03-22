using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumPad_Task : Task
{
    [Header("Task Specific")]
    Camera cam;

    public List<TextMeshProUGUI> randomNumbers;
    List<int> chosenRandomNumbers = new List<int>();
    int currentChecking = 0;

    public List<GameObject> numbers;

    public override void Start()
    {
        base.Start();
        cam = Camera.main;
    }

    int inputNumber;
    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && interacted && !locked)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                inputNumber = numbers.IndexOf(hit.transform.gameObject) + 1;

                if (inputNumber == chosenRandomNumbers[currentChecking])
                {
                    randomNumbers[currentChecking].color = Color.green;

                    if (currentChecking != 2)
                    {
                        currentChecking++;
                    }
                    else onTaskCompleted.Invoke();
                }
            }
        }

    }

    protected override void OnActivateTask()
    {
        base.OnActivateTask();
        resetNumbers();

        for (int i = 0; i < randomNumbers.Count; i++)
        {
            int rand = Random.Range(1, 10);
            chosenRandomNumbers.Add(rand);
            randomNumbers[i].text = rand.ToString();
        }
        currentChecking = 0;
    }

    protected override void OnTaskFailed()
    {
        base.OnTaskFailed();
        resetNumbers();
    }

    protected override void OnTaskCompleted()
    {
        base.OnTaskCompleted();
        resetNumbers();
    }

    void resetNumbers()
    {
        chosenRandomNumbers.Clear();
        for (int i = 0; i < randomNumbers.Count; i++)
        {
            randomNumbers[i].text = "";
            randomNumbers[i].color = Color.white;
        }
    }
}
