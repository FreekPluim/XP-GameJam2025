using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] GameObject fadeCanvas;
    [SerializeField] Image fadeImage;
    Color temp = Color.black;
    public float fadeSpeed = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FadeOut();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            FadeIn();
        }

    }

    void FadeOut()
    {
        fadeCanvas.SetActive(true);
        while (temp.a != 1)
        {
            temp.a = Mathf.Lerp(0, 1, fadeSpeed * Time.deltaTime);
            fadeImage.color = temp;
        }
    }

    void FadeIn()
    {
        while (temp.a != 0)
        {
            temp.a = Mathf.Lerp(1, 0, fadeSpeed * Time.deltaTime);
            fadeImage.color = temp;
        }
        fadeCanvas.SetActive(true);
    }
}
