using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static event Action beginGame;
    public static event Action snowglobeMode;
    public static event Action decorMode;
    public static event Action takePhoto;
    public static event Action mouseScrollUp;
    public static event Action mouseScrollDown;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public static GameManager Instance { get { return instance; } }


    public static void DoBeginGame()
    {
        if (beginGame != null)
        {
            beginGame();
        }
    }

    public static void DoSnowglobeMode()
    {
        if (snowglobeMode != null)
        {
            snowglobeMode();
        }
    }

    public static void DoDecorMode()
    {
        if (decorMode != null)
        {
            decorMode();
        }
    }

    public static void DoTakePhoto()
    {
        if (takePhoto != null)
        {
            takePhoto();
        }
    }

    public static void DoMouseScrollUp()
    {
        if (mouseScrollUp != null)
        {
            mouseScrollUp();
        }
    }

    public static void DoMouseScrollDown()
    {
        if (mouseScrollDown != null)
        {
            mouseScrollDown();
        }
    }
}