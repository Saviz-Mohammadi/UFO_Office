using UnityEngine;

public class AudioManager : MonoBehaviour {
    // TODO (SAVIZ): Make an event that interactables can subscribe to and listen to changes for audio.
    // TODO (SAVIZ): Make sure to either set the initial value of the voluem in awake and use that if reading from volume settings saved.
    
    public static AudioManager Instance;

    [SerializeField] private AudioSource _audioSource;
    
    public float _musicAudioVolume = 0.35f;
    public float _sfxAudioVolume = 0.35f;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ChangedMusicVolume(float volume) {
        this._musicAudioVolume = volume;
        
        _audioSource.volume = volume;
    }

    public void ChangedSfxVolume(float volume) {
        this._sfxAudioVolume = volume;
        
        // Invoke something and let others know the sound volume has changed.
    }
}
