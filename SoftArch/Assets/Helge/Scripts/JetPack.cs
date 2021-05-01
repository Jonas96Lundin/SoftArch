using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    CharController controller;
    [SerializeField] bool leftClickEffectEnabled = true, rightClickEffectEnabled = true;

    void Awake() => controller = GetComponent<CharController>();

    // Update is called once per frame
    void Update()
    {
        // Move up
        if (leftClickEffectEnabled && Input.GetButtonDown("Fire1") && controller != null)
            controller.AddForce(new Vector3(0, 500));

        // Move right
        if (rightClickEffectEnabled && Input.GetButtonDown("Fire2") && controller != null)
            controller.AddForce(new Vector3(1000, 0));
    }
}
