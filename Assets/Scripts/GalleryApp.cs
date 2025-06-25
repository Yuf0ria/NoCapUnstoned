using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class GalleryApp : MonoBehaviour
{
<<<<<<< Updated upstream
    Vector2 scrollDir;
    float scrollSpeed = 0.25f;
=======
    [SerializeField] private float scrollWheelSpeed = 50f;
    [SerializeField] private float scrollDragSpeed = 0.0025f;
>>>>>>> Stashed changes

    float scrollUpperLimit = 0;
    float scrollLowerLimit = 6.25f;
    [SerializeField] GameObject scrollObject;
    [SerializeField] Rigidbody2D scrollRigidBody;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dragScroll();
    }

    void mouseScroll()
    {
        
        Vector2 scrollDir;

        scrollDir = Mouse.current.scroll.ReadValue();
        if (scrollDir.y > 0 && scrollObject.transform.position.y <= scrollLowerLimit)
            scrollObject.transform.position += new Vector3(0, scrollWheelSpeed, 0);

        if (scrollDir.y < 0 && scrollObject.transform.position.y >= scrollUpperLimit)
            scrollObject.transform.position -= new Vector3(0, scrollWheelSpeed, 0);
    }


    
    float mousePosDown;

    void dragScroll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosDown = Input.mousePosition.y; // On Press, get the coordinate of the Mouse Position
        }

        else if (Input.GetMouseButtonUp(0))
        {
            
            scrollRigidBody.linearVelocityY = 0;
        }

        else if (Input.GetMouseButton(0))
        {
            if (mousePosDown != Input.mousePosition.y) // While not released, compare the record coordinates to the current coordinates for changes, if different from record, scroll
            {
                float scrollDir = (Input.mousePosition.y - mousePosDown);

                scrollRigidBody.linearVelocityY = scrollDir * scrollDragSpeed;

                Debug.Log(scrollDir);
            }
        }
    }
}
