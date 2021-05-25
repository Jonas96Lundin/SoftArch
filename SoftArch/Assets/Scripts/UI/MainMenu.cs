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

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("JonasLevel", LoadSceneMode.Single);
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
}
