using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] private AudioSource shipShootAudioSource;
    [SerializeField] private AudioSource shipEngineAudioSource;
    [SerializeField] private AudioSource musicSource;

    [Header("--- Audio Clip ---")]
    [SerializeField] private AudioClip[] shipShootingSounds;
    // 0 - Regular shot
    // 1 - Shotgun shot

    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private AudioClip engineThrustClip;

    private void Start()
    {
        musicSource.clip = backgroundMusicClip;
        musicSource.Play();
    }

    public void PlayShipShotClip(int typeOfShot)
    {
        shipShootAudioSource.clip = shipShootingSounds[typeOfShot];
        shipShootAudioSource.Play();
    }

    public void TurnOnEngine(float startTimeFlight)
    {
        shipEngineAudioSource.clip = engineThrustClip;
        shipEngineAudioSource.Play();
        shipEngineAudioSource.DOFade(.5f, startTimeFlight);
    }

    public void TurnOffEngine()
    {
        shipEngineAudioSource.DOFade(0f, 1f);
        shipEngineAudioSource.Stop();
    }
}
