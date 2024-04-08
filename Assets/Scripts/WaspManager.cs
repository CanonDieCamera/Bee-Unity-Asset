using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

/*
This is the Manager of the Wasp Enemy. It does:
    - Wasp Moving
        - simple AI: Wasp moves horizontal. If it collides with Terrain it changes direction.
        - other AI: Wasp follows a path of GameObjects
    - Stabilizing the Wasp's rotation if it changes uncontrolled
*/

public class WaspManager : MonoBehaviour
{
    //Movement
    [SerializeField]    //Shows a private variable defined directly under this line in the Inspector
    private float movingSpeed = 5f; //Moving speed of the Wasp, can be changed in the Inspector
    private bool faceRight = false;     //Saves if Wasp faces right
    private Vector2 direction = new Vector2(-1, 0);  //Moving direction of the Wasp
    private Vector3 lastPosition;

    [Header("Enemy AI Settings")]
    [SerializeField]
    private List<GameObject> points = new List<GameObject>(); //List of Points the Wasp should follow
    private int pointsIndex = 0;    //Index to loop over points List
    private bool simpleAI = false;  //Bool to determine AI System


    // Start is called before the first frame update
    void Start()
    {
        //If no points are defined Wasp uses simpleAI System
        if(points.Count == 0)
        {
            simpleAI = true;
        }

        //Freeze Rotation so it gets not affected by Physics simulation
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        //initialize lastPosition for UpdateRotation function
        lastPosition = transform.position;
        Debug.Log(gameObject.name + ": " + simpleAI);
    }

    // Update is called once per frame
    void Update()
    {
        if (simpleAI)
        {
            transform.Translate(direction * movingSpeed * Time.deltaTime);
        }
        else
        {
            FollowPath();
            UpdateRotation();
        }
    }

    //Is called once, if Wasp collides with something with a Collider2D
    private void OnCollisionEnter2D(Collision2D other) { 
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

    //Follows a path from the points List
    private void FollowPath()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[pointsIndex].transform.position, movingSpeed * Time.deltaTime);

        if(transform.position == points[pointsIndex].transform.position)
        {
            if(pointsIndex < points.Count - 1)
            {
                pointsIndex++;
            }
            else
            {
                pointsIndex = 0;
            }
        }
    }

    private void UpdateRotation()
    {
        if(transform.position.x > lastPosition.x && !faceRight) //moves to the right
        {
            transform.localEulerAngles = new Vector2(0, 180);
            faceRight = true;
        }
        else if(transform.position.x <  lastPosition.x && faceRight)
        {
            transform.localEulerAngles = new Vector2(0, 0);
            faceRight = false;
        }

        //Update lastPosition
        lastPosition = transform.position;
    }
}
