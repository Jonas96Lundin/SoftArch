using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    bool pauseMenuActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        pauseMenuActive = false;
        pauseMenu.SetActive(pauseMenuActive);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void ActivatePauseMenu()
    {
        pauseMenuActive = !pauseMenuActive;
        pauseMenu.SetActive(pauseMenuActive);
    }
}
