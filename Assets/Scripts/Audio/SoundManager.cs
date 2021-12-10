using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This class is a high-level audio manager for playing back soundsin the game. In has options for
/// playing sounds in 2D or 3D space, as well an controls for volume and mixer settings.
/// </summary>

//Struct containing the key audio properties we want to adjust per sound effect
[System.Serializable]
public struct GameSound
{
    public string name;
    [Range(0.0f, 1.0f)] public float volume;
    [Range(-3.0f, 3.0f)] public float pitch;
    [Range(0.0f, 1.0f)] public float spatialBlend;
    public AudioMixerGroup mixerGroup;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    private static Dictionary<string, List<AudioClip>> soundPool = new Dictionary<string, List<AudioClip>>();
    [SerializeField] AudioMixerGroup mixerGroup;
    [SerializeField] AudioMixer mixer;
    [SerializeField] SoundPlayer soundPlayerPrefab;
    private Transform listenerTransform;

    //Initialise variables; load audio resources (from 'Resources/Audio' folder) and hash file names into Dictionary
    //Fred B
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }

        listenerTransform = Camera.main.transform;
        AudioClip[] foundSounds = Resources.LoadAll<AudioClip>("Audio");

        //Loop through all sounds and find if audio files have a '_Random' tag
        //These sounds are grouped for randomised playback
        if (foundSounds != null)
        {
            for (int i = 0; i < foundSounds.Length; i++)
            {
                if (foundSounds[i] != null)
                {
                    string clipName = foundSounds[i].name.ToLower();

                    if (clipName.Contains("_random"))
                    {
                        string[] splitStrings = clipName.Split('_');
                        clipName = "";

                        for (int j = 0; j < splitStrings.Length; j++)
                        {
                            if (splitStrings[j].Contains("random"))
                                break;

                            if (j != 0)
                                clipName += "_";

                            clipName += splitStrings[j];
                        }

                        if (soundPool.ContainsKey(clipName))
                        {
                            soundPool.TryGetValue(clipName, out List<AudioClip> value);

                            if (value == null)
                            {
                                value = new List<AudioClip>();
                            }

                            value.Add(foundSounds[i]);
                        }
                        else
                        {
                            List<AudioClip> newValue = new List<AudioClip>();
                            newValue.Add(foundSounds[i]);
                            soundPool.Add(clipName, newValue);
                        }
                    }
                    else
                    {
                        if (!soundPool.TryGetValue(foundSounds[i].name.ToLower(), out List<AudioClip> value))
                        {
                            List<AudioClip> newValue = new List<AudioClip>();
                            newValue.Add(foundSounds[i]);
                            soundPool.Add(foundSounds[i].name.ToLower(), newValue);
                        }
                    }
                }
            }
        }
    }

    //Plays 2D AudioSources (i.e. those with 0 Spatial Blend)
    //Fred B
    public void PlaySound2D(GameSound gameSound)
    {
        SoundPlayer sp = GameObject.Instantiate(soundPlayerPrefab);
        if (sp != null)
            sp.StartPlayingSound(gameSound);
    }

    //Plays 3D AudioSources (i.e. those with >0 Spatial Blend that are parented to transforms in the scene)
    //Tim D
    public SoundPlayer PlaySound3D(GameSound gameSound)
    {
        SoundPlayer sp = GameObject.Instantiate(soundPlayerPrefab);
        if (sp != null)
        {
            sp.StartPlayingSound(gameSound);
            return sp;
        }
        return null;
    }

    //Returns an audioclip according to a GameSound name
    //Fred B
    public AudioClip GetAudioClipForGameSound(string name)
    {
        if (soundPool.TryGetValue(name.ToLower(), out List<AudioClip> value))
        {
            return value[UnityEngine.Random.Range(0, value.Count)];
        }
        return null;
    }

    //Returns an audioclip according to a GameSound
    //Fred B
    public AudioClip GetAudioClipForGameSound(GameSound gameSound)
    {
        if (soundPool.TryGetValue(gameSound.name.ToLower(), out List<AudioClip> value))
        {
            return value[UnityEngine.Random.Range(0, value.Count)];
        }
        return null;
    }

    //Fade in SFX
    //Tim D
    public void FadeInSFX(float fadeDuration)
    {
        mixer.DOSetFloat("Volume", 0, fadeDuration);
    }

    //Fade out SFX
    //Tim D
    public void FadeOutSFX(float fadeDuration)
    {
        mixer.DOSetFloat("Volume", -80.0f, fadeDuration);
    }

    //Fade SFX to a specified value
    //Tim D
    public void FadeSFXTo(float value, float fadeDuration)
    {
        mixer.DOSetFloat("Volume", value, fadeDuration);
    }

    //Sets SFX to a specified value
    //Tim D
    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 20.0f);
    }

    //Returns the volume in decibels (Db)
    //Tim D
    public float GetVolumeLogarithmic(string mixerGroupName)
    {
        float value;
        bool result = mixer.GetFloat(mixerGroupName, out value);

        if (result)
            return value;
        else
            return 0.0f;
    }

    //Returns the volume on a linear scale
    //Tim D
    public float GetVolumeLinear(string mixerGroupName)
    {
        return Mathf.Pow(10.0f, GetVolumeLogarithmic(mixerGroupName) / 20.0f);
    }

    //Sets music channel group to a specified value
    //Tim D
    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
    }

    //Returns this instance of the SoundManager; used for calling PlaySound functions
    //Tim D
    public static SoundManager Instance { get => instance; }
}
