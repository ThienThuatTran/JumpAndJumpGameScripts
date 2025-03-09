using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource audioSourceObject;
    public AudioClip hurtSFX;
    public AudioClip collectItemSFX;
    public AudioClip deathSFX;
    public AudioClip jumpSFX;
    public AudioClip hitSFX;
    public AudioClip wallJumpSFX;
    public AudioClip fallingSFX;
    public AudioClip clickSFX;
    public AudioClip levelCompleteSFX;
    public AudioClip powerUpSFX;
    public AudioClip dashSFX;
    public AudioClip starPopupSFX;
    public AudioClip starRateCompleteSFX;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayeSoundFX(AudioClip audioClip, Transform soundTransform, float volume)
    {
        AudioSource audioSource = Instantiate(audioSourceObject, soundTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;    

        float audioLength = audioSource.clip.length;

        audioSource.Play();
        Destroy(audioSource.gameObject, audioLength);
    }
}
