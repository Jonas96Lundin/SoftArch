using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        //optionsMenu.SetActive(true);
        //optionsMenu.SetActive(false);
        AudioManager.instance.Play("MenuMusic");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne", LoadSceneMode.Single);
        AudioManager.instance.Stop("MenuMusic");
        //AudioManager.instance.Play("LevelMusic");
        AudioManager.instance.Play("DystopicSound");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioManager.instance.SetVolume(volume);
    }
    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SetVolumeSFX(volume);
    }
}
