using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public bool playAudio;

    [Header("Audio Sources")]
    public AudioSource playerAudioSource;
    AudioSource playerGunAudioSource;
    [Header("Audio Clips")]
    public AudioClip sharpenerClip;
    public AudioClip sniperGunClip;

    public void PlayGunSound(string gunType)
    {
        if (playAudio)
        {
            if (gunType == "Sharpener")
            {
                playerGunAudioSource.PlayOneShot(sharpenerClip);
            }
            if (gunType == "Sniper Gun")
            {
                playerGunAudioSource.PlayOneShot(sniperGunClip);
            }
        }
    }

    public void SetPlayerGunAudioSource(AudioSource gunSource)
    {
        playerGunAudioSource = gunSource;
    }
}
