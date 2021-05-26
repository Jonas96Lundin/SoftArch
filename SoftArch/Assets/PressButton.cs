using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    [SerializeField]
    GameObject leftDoor;
    [SerializeField]
    GameObject rightDoor;

    //private Material material;
    // Start is called before the first frame update
    void Start()
    {
        //material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    private Material material;

	private void OnTriggerEnter(Collider other)
	{
        material.SetColor("_EmissionColor", Color.green * Mathf.Pow(2, 3 - 0.4169F));
        leftDoor.transform.SetPositionAndRotation(leftDoor.transform.position, Quaternion.Euler(0, 0, 0));
        rightDoor.transform.SetPositionAndRotation(rightDoor.transform.position, Quaternion.Euler(0, 0, 0));

    }

	private void OnTriggerExit(Collider other)
	{
        material.SetColor("_EmissionColor", Color.red * Mathf.Pow(2, 3 - 0.4169F));
        leftDoor.transform.SetPositionAndRotation(leftDoor.transform.position, Quaternion.Euler(0, 180, 0));
        rightDoor.transform.SetPositionAndRotation(rightDoor.transform.position, Quaternion.Euler(0, 180, 0));
    }
}
