using UnityEngine;

/// <summary>
/// This class manages our SoundPlayer objects, which are spawned in the scene by
/// SoundManager. These objects handle their own playback and destruction.
/// </summary>

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource;
    private bool startedPlaying = false;

    //Check whether a sound has finished playing and sets self-destruct timer
    //Fred B
    void Update()
    {
        if (startedPlaying && !sfxAudioSource.isPlaying)
        {
            StartCoroutine(DestroyDelay());
        }
    }

    //Destroy the object after a delay
    //Fred B
    private System.Collections.IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject.Destroy(this.gameObject);
    }

    //Apply properties and play the GameSound
    //Fred B
    public SoundPlayer StartPlayingSound(GameSound gs)
    {
        if (startedPlaying) return this;
        startedPlaying = true;
        AudioClip ac = SoundManager.Instance.GetAudioClipForGameSound(gs);
        if (ac == null)
        {
            sfxAudioSource.Stop();
            return this;
        }
        sfxAudioSource.playOnAwake = false;
        sfxAudioSource.dopplerLevel = 0;
        sfxAudioSource.volume = gs.volume;
        sfxAudioSource.pitch = gs.pitch;
        sfxAudioSource.spatialBlend = gs.spatialBlend;
        sfxAudioSource.clip = ac;

        if (gs.mixerGroup != null)
            sfxAudioSource.outputAudioMixerGroup = gs.mixerGroup;

        sfxAudioSource.PlayOneShot(sfxAudioSource.clip);
        return this;
    }

    //Play the audio clip
    //Fred B
    public SoundPlayer StartPlayingSound(AudioClip clip)
    {
        if (startedPlaying) return this;
        if (clip == null) return this;
        startedPlaying = true;
        sfxAudioSource.clip = clip;
        sfxAudioSource.playOnAwake = false;
        sfxAudioSource.dopplerLevel = 0;
        sfxAudioSource.PlayOneShot(sfxAudioSource.clip);
        return this;
    }
}