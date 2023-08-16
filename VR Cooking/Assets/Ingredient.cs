using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    float cooked = 0;
    float previousCooked = 0;
    float tempTimer = 0;

    bool cooking = false;

    Renderer _renderer = null;
    Material _material = null;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }

    void Update()
    {
        if (!cooking) { return; }

        cooked += Math.Clamp(Time.deltaTime * .0001f, 0, 1);

        if (cooked != previousCooked)
        {
            previousCooked = cooked;

            _material.color = new Color(_material.color.r * (1 - cooked), _material.color.g * (1 - cooked), _material.color.b * (1 - cooked), _material.color.a);
        }        
    }

    void OnCollisionEnter(Collision collision)
    {
        cooking = false;
        if (collision.gameObject.tag == "Pan")
        {
            if (collision.gameObject.GetComponent<Pans>().onStove == true)
            {
                cooking = true;
            }
        }
    }
}