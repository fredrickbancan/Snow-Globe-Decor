using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inGameHud;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Camera mainMenuCam;
    [SerializeField] private Camera playerGlobeCam;
    [SerializeField] private Camera playerTableTopCam;
    [SerializeField] private Camera tweenCam;
    [SerializeField] private float camTweenSpeed = 2.0F;
    private Camera currentCam;
    private static GameManager instance = null;
    public static event Action beginGame;
    public static event Action snowglobeMode;
    public static event Action tableTopMode;
    public static event Action camTweenStart;
    public static event Action takePhoto;
    public static event Action weaponChangeNext;
    public static event Action weaponChangePrev;

    private bool gamePaused = false;
    private bool inGameHudVisible = true;
    private bool tableTopViewing = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(pauseMenu);
            DontDestroyOnLoad(inGameHud);
            DontDestroyOnLoad(mainMenu);
            pauseMenu.SetActive(false);
            inGameHud.SetActive(false);
            UnPauseGame();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currentCam = mainMenuCam;
            mainMenuCam.enabled = true;
            playerGlobeCam.enabled = false;
            playerTableTopCam.enabled = false;
            tweenCam.enabled = false;
            return;
        }
        Destroy(pauseMenu);
        Destroy(inGameHud);
        Destroy(mainMenu);
        Destroy(this.gameObject);
    }

    public static void Instance_ToggleTableView()
    {
        instance.ToggleTableView();
    }

    public void ToggleTableView()
    {
        if(tableTopViewing)
        {
            tableTopViewing = false;
            UnPauseGame();
            pauseMenu.SetActive(false);
            inGameHud.SetActive(false);
            mainMenu.SetActive(false);
            currentCam = playerTableTopCam;
            tweenCam.transform.position = currentCam.transform.position;
            tweenCam.transform.rotation = currentCam.transform.rotation;
            tweenCam.transform.DOMove(playerGlobeCam.transform.position, camTweenSpeed).SetEase(Ease.InOutCubic).OnStart(SwapToTweenCam).OnComplete(SwapToPlayerCam);
            tweenCam.transform.DOLocalRotate(playerGlobeCam.transform.rotation.eulerAngles, camTweenSpeed).SetEase(Ease.InOutCubic);
        }
        else
        {
            tableTopViewing = true;
            UnPauseGame();
            pauseMenu.SetActive(false);
            inGameHud.SetActive(false);
            mainMenu.SetActive(false);
            currentCam = playerGlobeCam;
            tweenCam.transform.position = currentCam.transform.position;
            tweenCam.transform.rotation = currentCam.transform.rotation;
            tweenCam.transform.DOMove(playerTableTopCam.transform.position, camTweenSpeed).SetEase(Ease.InOutCubic).OnStart(SwapToTweenCam).OnComplete(SwapToTableTopCam);
            tweenCam.transform.DOLocalRotate(playerTableTopCam.transform.rotation.eulerAngles, camTweenSpeed).SetEase(Ease.InOutCubic);
        }
    }

    public static void Instance_ProvideCameras(Camera mainMenuCam, Camera playerCam, Camera tableCam, Camera tweenCam)
    {
        instance.ProvideCameras( mainMenuCam, playerCam, tableCam,tweenCam);
    }

    public void ProvideCameras(Camera mainMenuCam, Camera playerCam, Camera tableCam, Camera tweenCam)
    {
        this.mainMenuCam = mainMenuCam;
        playerGlobeCam = playerCam;
        playerTableTopCam = tableCam;
        this.tweenCam = tweenCam;
    }

    public static void Instance_QuitToMainMenu()
    {
        instance.QuitToMainMenu();
    }

    public void QuitToMainMenu()
    {
        UnPauseGame();
        pauseMenu.SetActive(false);
        inGameHud.SetActive(false);
        tweenCam.transform.position = currentCam.transform.position;
        tweenCam.transform.rotation = currentCam.transform.rotation;
        tweenCam.transform.DOMove(mainMenuCam.transform.position, camTweenSpeed).SetEase(Ease.InOutCubic).OnStart(SwapToTweenCam).OnComplete(SwapToMainMenuCam).OnComplete(ReloadWorldAfterQuitToMainMenu);
        tweenCam.transform.DOLocalRotate(mainMenuCam.transform.rotation.eulerAngles, camTweenSpeed).SetEase(Ease.InOutCubic);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;;
    }

    private void ReloadWorldAfterQuitToMainMenu()
    {
        mainMenu.SetActive(true);
        ReloadWorld();
    }

    public static void Instance_EnterSnowGlobe()
    {
        instance.EnterSnowGlobe();
    }

    public void EnterSnowGlobe()
    {
        UnPauseGame();
        pauseMenu.SetActive(false);
        inGameHud.SetActive(true);
        mainMenu.SetActive(false);
        currentCam = mainMenuCam;
        tweenCam.transform.position = currentCam.transform.position;
        tweenCam.transform.rotation = currentCam.transform.rotation;
        tweenCam.transform.DOMove(playerGlobeCam.transform.position, camTweenSpeed).SetEase(Ease.InOutCubic).OnStart(SwapToTweenCam).OnComplete(SwapToPlayerCam);
        tweenCam.transform.DOLocalRotate(playerGlobeCam.transform.rotation.eulerAngles, camTweenSpeed).SetEase(Ease.InOutCubic);
    }

    private void SwapToTweenCam()
    {
        mainMenuCam.enabled = false;
        playerGlobeCam.enabled = false;
        playerTableTopCam.enabled = false;
        tweenCam.enabled = true;
        currentCam = tweenCam;
        DoCamTweenStart();
    }

    private void SwapToMainMenuCam()
    {
        mainMenuCam.enabled = true;
        playerGlobeCam.enabled = false;
        playerTableTopCam.enabled = false;
        tweenCam.enabled = false;
        currentCam = mainMenuCam;
    }

    private void SwapToPlayerCam()
    {
        mainMenuCam.enabled = false;
        playerGlobeCam.enabled = true;
        playerTableTopCam.enabled = false;
        tweenCam.enabled = false;
        currentCam = playerGlobeCam;
        DoBeginGame();
    }

    private void SwapToTableTopCam()
    {
        mainMenuCam.enabled = false;
        playerGlobeCam.enabled = false;
        playerTableTopCam.enabled = true;
        tweenCam.enabled = false;
        currentCam = playerTableTopCam;
        DoTableTopMode();
    }

    public static void Instance_SetGraphicsLevel(GraphicsTier tier)
    {
        instance.SetGraphicsLevel(tier);
    }

    public void SetGraphicsLevel(GraphicsTier tier)
    {
        SpawnableLight.SetGraphicsLevel(tier);
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

    public void ReloadWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public static void DoCamTweenStart()
    {
        if (camTweenStart != null)
        {
            camTweenStart();
        }
    }

    public static void DoTableTopMode()
    {
        if (tableTopMode != null)
        {
            tableTopMode();
        }
    }

    public static void DoTakePhoto()
    {
        if (takePhoto != null)
        {
            takePhoto();
        }
    }

    public static void DoWeaponScrollNext()
    {
        if (weaponChangeNext != null)
        {
            weaponChangeNext();
        }
    }

    public static void DoWeaponScrollPrev()
    {
        if (weaponChangePrev != null)
        {
            weaponChangePrev();
        }
    }
}