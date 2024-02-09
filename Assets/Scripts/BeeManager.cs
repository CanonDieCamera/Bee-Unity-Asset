using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    /*
    This is the manager of the player controllable bee. It does:
        - controll the bee.
    */

    //Movement
    [SerializeField]    //Shows a private variable defined directly under this line in the Inspector
    private float movingSpeed = 5f; // Moving Speed of the Bee, can be changed in the Inspector
    private Vector2 direction; //Direction of the player controlled movement
    private bool faceRight = false; //Saves if Bee faces right

    //Animation
    private Animator moveAnimation;

    // Start is called before the first frame update
    private void Start() {
        moveAnimation = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        //reset Animation
        moveAnimation.SetBool("Forward", false);
        moveAnimation.SetBool("Up", false);
        moveAnimation.SetBool("Down", false);

        //Controll the Bee with 'w', 'a', 's', 'd'
        if(Input.GetKey(KeyCode.W))   //KeyCodes at: https://docs.unity3d.com/ScriptReference/KeyCode.html
        {
            direction = new Vector2(0, 1);
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            //Animation
            moveAnimation.SetBool("Up", true);
        }

        if(Input.GetKey(KeyCode.A))
        {
            direction = new Vector2(-1, 0);
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            if (faceRight) //If bee faces to right side it will get rotated around y Axis, so it faces to left side
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                faceRight = false;
            }

            //Animation
            moveAnimation.SetBool("Forward", true);
        }

        if(Input.GetKey(KeyCode.S))
        {
            direction = new Vector2(0, -1);
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            //Animation
            moveAnimation.SetBool("Down", true);
        }

        if(Input.GetKey(KeyCode.D))
        {
            direction = new Vector2(-1, 0);
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            //Rotation
            if (!faceRight) //If bee faces to not to right side it will get rotated around y Axis, so it faces to right side
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                faceRight = true;
            }

            //Animation
            moveAnimation.SetBool("Forward", true);
        }
    }
}
