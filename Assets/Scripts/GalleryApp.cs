using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering;

public class GalleryApp : MonoBehaviour
{
    Vector2 scrollDir;
    [SerializeField] private float scrollSpeed = 50f;

    float scrollUpperLimit = 200f;
    float scrollLowerLimit = 1700f;
    [SerializeField] GameObject scrollObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scrollUpperLimit = scrollObject.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        scrollDir = Mouse.current.scroll.ReadValue();
        if (scrollDir.y > 0 && scrollObject.transform.position.y <= scrollLowerLimit)
            scrollObject.transform.position += new Vector3(0,scrollSpeed,0);
            
        if (scrollDir.y < 0  && scrollObject.transform.position.y >= scrollUpperLimit)
            scrollObject.transform.position -= new Vector3(0, scrollSpeed, 0);
    }
}
