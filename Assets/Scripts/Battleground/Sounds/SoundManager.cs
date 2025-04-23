using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set;  }

    [SerializeField] private List<AudioSource> _SFXChannels;
    [SerializeField] private AudioSource _musicChannel;

    [SerializeField] private List<AudioClip> _musicWarClips;
    [SerializeField] private List<AudioClip> _musicMapClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _SFXChannels.ForEach(channel => channel.Stop());
        _musicChannel.Stop();

        if (SceneManager.GetActiveScene().name == "Battleground")
        {
            PlayWarMusic();
        }
        else
        {
            PlayMapMusic();
        }
    }

    private void PlayWarMusic()
    {
        var randomMusicIndex = Random.Range(0, _musicWarClips.Count);
        _musicChannel.clip = _musicWarClips[randomMusicIndex];
        _musicChannel.Play();
    }

    private void PlayMapMusic()
    {
        var randomMusicIndex = Random.Range(0, _musicMapClips.Count);
        _musicChannel.clip = _musicMapClips[randomMusicIndex];
        _musicChannel.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        foreach (var channel in _SFXChannels)
        {
            if (!channel.isPlaying)
            {
                channel.PlayOneShot(clip);

                return;
            }
        }
    }

    public void ChangeSoundVolume(SoundType soundType, float value)
    {
        switch (soundType)
        {
            case SoundType.Music:
                _musicChannel.volume = value; 
                
                break;
            case SoundType.SFX:
                _SFXChannels.ForEach(channel => channel.volume = value);

                break;
        }
    }


    public float GetSoundVolume(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.Music => _musicChannel.volume,
            SoundType.SFX => _SFXChannels[0].volume,
            _ => 0,
        };
    }
}
