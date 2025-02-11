using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set;  }

    private AudioSource _infantryAttackChannel;

    [SerializeField] private List<AudioClip> _infantryAttackClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _infantryAttackChannel = GetComponent<AudioSource>();
        _infantryAttackChannel.volume = 0.1f;
        _infantryAttackChannel.playOnAwake = false;
    }

    public void PlayInfantryAttackSound()
    {
        if (!_infantryAttackChannel.isPlaying)
        {
            var randomClipIndex = Random.Range(0, _infantryAttackClips.Count);
            _infantryAttackChannel.PlayOneShot(_infantryAttackClips[randomClipIndex]);
        }
    }
}
