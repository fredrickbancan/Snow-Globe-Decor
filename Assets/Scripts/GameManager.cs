using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inGameHud;
    private static GameManager instance = null;
    public static event Action beginGame;
    public static event Action snowglobeMode;
    public static event Action decorMode;
    public static event Action takePhoto;
    public static event Action mouseScrollUp;
    public static event Action mouseScrollDown;

    private bool gamePaused = false;
    private bool inGameHudVisible = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(pauseMenu);
            DontDestroyOnLoad(inGameHud);
            pauseMenu.SetActive(false);
            UnPauseGame();
            return;
        }
        Destroy(this.gameObject);
    }

    public static void Instance_ToggleHudVisible()
    {
        instance.ToggleHudVisible();
    }

    public void ToggleHudVisible()
    {
        if (inGameHudVisible)
            HideHud();
        else
            MakeHudVisible();
    }

    public static void Instance_MakeHudVisible()
    {
        instance.MakeHudVisible();
    }

    public void MakeHudVisible()
    {
        inGameHudVisible = true;
        inGameHud.SetActive(true);
    }

    public static void Instance_HideHud()
    {
        instance.HideHud();
    }

    public void HideHud()
    {
        inGameHudVisible = false;
        inGameHud.SetActive(false);
    }


    public static bool Instance_GetIsPaused()
    {
        return instance.GetIsPaused();
    }
    public bool GetIsPaused()
    {
        return gamePaused;
    }

    //these static method clones are for ease of access by ui events
    public static void Instance_PauseGame()
    {
        instance.PauseGame();
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0.0F;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }

    public static void Instance_UnPauseGame()
    {
        instance.UnPauseGame();
    }

    public void UnPauseGame()
    {
        gamePaused = false;
        Time.timeScale = 1.0F;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }

    public static void Instance_TogglePauseGame()
    {
        instance.TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        if (gamePaused)
        {
            UnPauseGame();
            return;
        }

        PauseGame();
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