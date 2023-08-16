using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pans : MonoBehaviour
{
    public Component[] colliders;
    public bool onStove = false;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stove")
        {
            Debug.Log("On Stove");
            onStove = true;
        }
    }
}
