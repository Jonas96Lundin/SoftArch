using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
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
        material.color = Color.green;
    }

	private void OnTriggerExit(Collider other)
	{
        material.color = Color.red;
    }
}
