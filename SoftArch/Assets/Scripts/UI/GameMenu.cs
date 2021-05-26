using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    GameObject optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeInHierarchy)
            {
                optionsMenu.SetActive(false);
            }
            else
            {
                optionsMenu.SetActive(true);
            }
        }
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        AudioManager.instance.Stop("DystopicSound");
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
