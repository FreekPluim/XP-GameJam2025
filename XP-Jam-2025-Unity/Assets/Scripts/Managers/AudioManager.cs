using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //data
    public UnityDictionary<string, Sound> sounds;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            InitializeSounds();
        }
    }

    private void InitializeSounds()
    {
        foreach (Sound s in sounds.Values)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.SetupSource();
        }
    }

    //=============== play sound fx ===============
    public void Play(string name)
    {
        if (sounds.ContainsKey(name))
        {
            sounds[name].source.Play();
        }
        //debug info
        else { Debug.LogError(transform.name + " contains no sound for " + name); }
    }

    public void PlayOneShot(string name)
    {
        if (sounds.ContainsKey(name))
        {
            sounds[name].source.PlayOneShot(sounds[name].clip);
        }
        //debug info
        else { Debug.LogError(transform.name + " contains no sound for " + name); }
    }
}
