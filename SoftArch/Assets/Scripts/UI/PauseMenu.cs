using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    CharController cc;

    bool pauseMenuActive;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivatePauseMenu();
        }
        
        // TEST to activate vsync + rendering at only 60 fps
        if (Input.GetKeyDown(KeyCode.V))
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
            Debug.Log("Vsync active");
        }
    }

    public void SetVolume(float volume)
    {
        AudioManager.instance.SetVolume(volume);
    }

    public void ClosePauseMenu()
    {
        cc.UnPause();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuActive = false;
        pauseMenu.SetActive(pauseMenuActive);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        Debug.Log("GOT CLICKED");
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void ActivatePauseMenu()
    {
        cc.Pause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuActive = !pauseMenuActive;
        pauseMenu.SetActive(pauseMenuActive);
    }
}
