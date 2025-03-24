using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calculator_Task : Task
{
    [Header("Task Specific")]
    Camera cam;

    public TextMeshProUGUI Calculation;
    public TextMeshProUGUI Answer;

    string input;
    int inputNumber;
    int answer;

    public List<GameObject> numbers;

    public override void Start()
    {
        base.Start();
        cam = Camera.main;
    }

    protected override void OnActivateTask()
    {
        base.OnActivateTask();

        //Create calculation
        int num1 = Random.Range(2, 10);
        int num2 = Random.Range(1, 10);

        if (num1 + num2 == 10) num1 -= 1;
        Calculation.text = $"{num1} + {num2}";
        answer = num1 + num2;
    }

    protected override void OnTaskCompleted(Task task)
    {
        base.OnTaskCompleted(task);

        Calculation.text = "";
        answer = 0;
        Answer.text = "";
        input = "";
    }
    protected override void OnTaskFailed(Task task)
    {
        base.OnTaskFailed(task);

        Calculation.text = "";
        answer = 0;
        Answer.text = "";
        input = "";
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && interacted && !locked)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                AudioManager.instance.PlayOneShot("Click");
                inputNumber = numbers.IndexOf(hit.transform.gameObject) + 1;
                input += inputNumber.ToString();
                Answer.text = input;

                if (input.Length == answer.ToString().Length)
                {
                    if (int.TryParse(input, out int res))
                    {
                        if (res == answer)
                        {
                            onTaskCompleted.Invoke(this);
                        }
                        else
                        {
                            AudioManager.instance.PlayOneShot("Mistake");
                            input = "";
                            Answer.text = "";
                        }
                    }
                }



            }
        }
    }
}
