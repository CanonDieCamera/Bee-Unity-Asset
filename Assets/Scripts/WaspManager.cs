using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

/*
This is the Manager of the Wasp Enemy. It does:
    - 
*/

public class WaspManager : MonoBehaviour
{
    //Movement
    [SerializeField]    //Shows a private variable defined directly under this line in the Inspector
    private float movingSpeed = 5f; //Moving speed of the Wasp, can be changed in the Inspector
    private bool faceRight = false;     //Saves if Wasp faces right
    private Vector2 direction = new Vector2(-1, 0);  //Moving direction of the Wasp

    [SerializeField]
    private float maxMovingDistance = 10;

    [Header("Enemy AI Settings")]
    private bool simpleAI = true;
    //Stabilization
    private float damping = 2;
    private bool currentlyStabalizing = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (simpleAI)
        {
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            //Stabilizing the bee so that it always brings itself into a horizontal position
            Stabilizing();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {    //If Wasp collides with something with a Collider2D
        if(other.collider.tag == "Terrain")
        {
            if(simpleAI)
            {
                //Flip direction
                if (faceRight)
                {
                    transform.localEulerAngles = new Vector2(0, 0);
                    faceRight = false;
                }
                else{
                    transform.localEulerAngles = new Vector2(0, 180);
                    faceRight = true;
                }
            }
        }
    }

    private void Stabilizing()
    {
        if(currentlyStabalizing)
        {
            if(faceRight)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 180, 0), damping * Time.deltaTime);
                
                if(transform.localEulerAngles == new Vector3(0, 180, 0))
                {
                    currentlyStabalizing = false;
                }
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), damping * Time.deltaTime);
                
                if(transform.localEulerAngles == new Vector3(0, 0, 0))
                {
                    currentlyStabalizing = false;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        currentlyStabalizing = true;
    }
}
