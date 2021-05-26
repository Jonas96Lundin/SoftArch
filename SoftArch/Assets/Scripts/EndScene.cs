using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScene : MonoBehaviour
{
    [SerializeField]
    GameObject ship;

    [SerializeField]
    Canvas credits;

    float timeElapsed;
    float lerpDuration = 10;

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            ship.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(210, 0, 0), timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            credits.gameObject.SetActive(true);
        }
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
