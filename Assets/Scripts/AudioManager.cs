using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Sound {Shoot};
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    public static AudioManager Instance {get; private set;}
    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        //musicSource.clip = audioClips[(int)Sound.BGM];
        //musicSource.Play();
    }
    public void PlaySounds(Sound sound)
    {
        sfxSource.PlayOneShot(audioClips[(int)sound]);
    }
}
