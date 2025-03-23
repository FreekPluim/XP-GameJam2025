using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask mask;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask))
            {

                Task task = hit.transform.GetComponentInParent<Task>();

                if (task != null)
                {
                    task.interacted = true;
                    return;
                }
            }
        }
    }
}
