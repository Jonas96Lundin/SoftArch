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
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        LoadEndScene();
    }
}
