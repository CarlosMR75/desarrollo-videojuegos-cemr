using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContriolPlataforma : MonoBehaviour
{
    PlatformEffector2D pe2D;

    public bool leftPlatform;

    private float inputY;

    void Start()
    {
        pe2D = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        inputY = Input.GetAxisRaw("Vertical");
        
        if (inputY <= -1 && !leftPlatform)
        {
            pe2D.rotationalOffset = 180;

            leftPlatform = true;

            //gameObject.layer = 2;
        }   
    }

    private void OnCollisionExit2D(Collision2D other) {
        pe2D.rotationalOffset = 0;

        leftPlatform = false;

        //gameObject.layer = 6;
    }
}
