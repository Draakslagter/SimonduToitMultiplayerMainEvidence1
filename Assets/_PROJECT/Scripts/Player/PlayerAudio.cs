using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource =  GetComponent<AudioSource>();
    }

    public void PlayClip(int clip)
    {
        _audioSource.clip = audioClips[clip];
        _audioSource.Play();
    }
}
