using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// This class handles basic behaviours for music, including controls for 
/// fading in and delaying audio playback
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private bool fadeIn = true;
    [SerializeField] private float fadeDuration = 2.0f;
    [SerializeField] private bool delayStart = false;
    [SerializeField] private float delayDuration = 1.0f;
    [SerializeField] AudioSource mainMusic = null;
    [SerializeField] float mainMusicTargetVolume = 0.0f;
    //[SerializeField] AudioSource winMusic = null;
    //[SerializeField] AudioSource lossMusic = null;

    //Subscribe to events and initialise variables
    private void Awake()
    {
        //BattleEvents.encounterFinish += OnEncounterWin;
        //BattleEvents.playerDeath += OnEncounterLoss;
        mainMusic.volume = 0.0f;
    }

    private void Start()
    {
        if (delayStart)
        {
            StartCoroutine(StartMusicDelay());
        }
        else
        {
            PlayMusic();
        }
    }

    //Delay music playback
    private System.Collections.IEnumerator StartMusicDelay()
    {
        yield return new WaitForSeconds(delayDuration);
        PlayMusic();
    }

    //Play the AudioSource
    private void PlayMusic()
    {
        if (fadeIn)
        {
            mainMusic.Play();
            mainMusic.DOFade(mainMusicTargetVolume, fadeDuration).SetEase(Ease.InCubic);
        }
        else
        {
            mainMusic.volume = mainMusicTargetVolume;
            mainMusic.Play();
        }
    }

    //Unsubscribe from events
    private void OnDestroy()
    {
        //BattleEvents.encounterFinish -= OnEncounterWin;
        //BattleEvents.playerDeath -= OnEncounterLoss;
    }

    //Fade out music when encounter is finished
    private void OnEncounterWin()
    {
        mainMusic.DOFade(0.0f, 1.0f).SetEase(Ease.Linear);
    }

    //Fade out music when encounter is finished
    private void OnEncounterLoss()
    {
        mainMusic.DOFade(0.0f, 1.0f).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }
}