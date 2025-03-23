using System.Collections;
using TMPro;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    Camera cam;
    public bool activated = false;

    [SerializeField] DialogueManager dialogueManager;
    public TextMeshProUGUI text;

    [SerializeField] Material readyButtonMaterial;
    [SerializeField] Color deactive, active;

    RaycastHit hit;
    [SerializeField] LayerMask mask;
    Coroutine blink;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (activated && Input.GetMouseButtonDown(0) && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask))
        {
            AudioManager.instance.PlayOneShot("Click");

            StopCoroutine(blink);
            GameManager.instance.onReadyPressed?.Invoke();
            readyButtonMaterial.color = deactive;
            activated = false;
        }
    }
    public void ActivateButton()
    {
        activated = true;
        blink = StartCoroutine(Blink());
        dialogueManager.endOfDialoguePlay.RemoveListener(ActivateButton);
    }
    IEnumerator Blink()
    {
        while (true)
        {
            readyButtonMaterial.color = active;
            yield return new WaitForSeconds(0.6f);
            readyButtonMaterial.color = deactive;
            yield return new WaitForSeconds(0.6f);
        }
    }
}
