
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound : ISerializationCallbackReceiver
{
    public AudioClip clip;

    [Header("Settings")]
    public AudioMixerGroup output;
    [Range(0, 256)] public int priority;
    [Range(0f, 1f)] public float volume;
    [Range(0.1f, 3f)] public float pitch;

    [Header("Bypass Settings")]
    public bool bypassEffects;
    public bool bypassListenerEffects;
    public bool bypassReverbZones;

    [HideInInspector] public AudioSource source;
    //serialization
    [HideInInspector] public bool setBaseValues;

    public void SetupSource(AudioSource source = null)
    {
        if (!source) { source = this.source; } //use stored source by default
        source.clip = clip;
        //settings
        source.outputAudioMixerGroup = output;
        source.priority = priority;
        source.volume = volume;
        source.pitch = pitch;
        //bypass settings
        source.bypassEffects = bypassEffects;
        source.bypassListenerEffects = bypassListenerEffects;
        source.bypassReverbZones = bypassReverbZones;
    }

    //============ serialization ============
    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        if (!setBaseValues)
        {
            priority = 128;
            volume = 0.3f;
            pitch = 1f;
            setBaseValues = true;
        }
    }
}
