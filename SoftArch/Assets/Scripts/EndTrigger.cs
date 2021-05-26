using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{

    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadEndScene();

        }
    }
}
