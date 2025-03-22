using UnityEngine;

public class LightHandler : MonoBehaviour
{
    public GameObject lightObject;
    public Material lightMaterial;

    [SerializeField] Color lightOff;
    [SerializeField] Color lightOn;

    private void Start()
    {
        lightMaterial = new Material(lightMaterial);
        lightObject.GetComponent<Renderer>().material = lightMaterial;
    }
    public void TurnOn()
    {
        lightMaterial.color = lightOn;
        lightMaterial.EnableKeyword("_EMISSION");
    }
    public void TurnOff()
    {
        lightMaterial.color = lightOff;
        lightMaterial.DisableKeyword("_EMISSION");
    }
}
