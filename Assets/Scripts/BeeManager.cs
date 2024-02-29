using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

/*
This is the manager of the player controllable bee. It does:
    - controll the bee (Movement, Animation)
    - stabilizing the Bee's rotation if it changes uncontrolled
*/

public class BeeManager : MonoBehaviour
{
    //Movement
    [SerializeField]    //Shows a private variable defined directly under this line in the Inspector
    private float movingSpeed = 5f; // Moving Speed of the Bee, can be changed in the Inspector
    private Vector2 direction; //Direction of the player controlled movement
    private bool faceRight = false; //Saves if Bee faces right
    private Rigidbody2D mRigidbody2D;

    //Animation
    private Animator moveAnimation;
    [SerializeField]
    private Sprite deadSprite;

    //Status
    private bool isDead = false;
    private bool hasPollen = false;
    private GameObject pollen;

    //GameOver
    [SerializeField]
    private GameObject gameOver;

    // Start is called before the first frame update
    private void Start() {
        //Animations for Bee movement
        moveAnimation = transform.GetChild(0).GetComponent<Animator>();

        //Get rigidbody2D
        mRigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        //Freeze Rotation so it gets not affected by Physics simulation
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        //Get GameObject Pollen
        pollen = transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    private void Update() {
        //reset Animation
        moveAnimation.SetBool("Forward", false);
        moveAnimation.SetBool("Up", false);
        moveAnimation.SetBool("Down", false);

        //for pollen idle
        if(hasPollen && !pollen.transform.GetChild(0).gameObject.activeSelf)
        {
            pollen.transform.GetChild(0).gameObject.SetActive(true);
            pollen.transform.GetChild(1).gameObject.SetActive(false);
            pollen.transform.GetChild(2).gameObject.SetActive(false);
            pollen.transform.GetChild(3).gameObject.SetActive(false);
        }

        //Controll the Bee with 'w', 'a', 's', 'd'
        if(Input.GetKey(KeyCode.W) && !isDead)   //KeyCodes at: https://docs.unity3d.com/ScriptReference/KeyCode.html
        {
            direction = new Vector2(0, 1);
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            //Animation
            moveAnimation.SetBool("Up", true);

            //Pollen
            if(hasPollen && !pollen.transform.GetChild(1).gameObject.activeSelf)
            {
                pollen.transform.GetChild(0).gameObject.SetActive(false);
                pollen.transform.GetChild(1).gameObject.SetActive(true);
                pollen.transform.GetChild(2).gameObject.SetActive(false);
                pollen.transform.GetChild(3).gameObject.SetActive(false);
            }
        }

        if(Input.GetKey(KeyCode.A) && !isDead)
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

            //Pollen
            if(hasPollen && !pollen.transform.GetChild(3).gameObject.activeSelf)
            {
                pollen.transform.GetChild(0).gameObject.SetActive(false);
                pollen.transform.GetChild(1).gameObject.SetActive(false);
                pollen.transform.GetChild(2).gameObject.SetActive(false);
                pollen.transform.GetChild(3).gameObject.SetActive(true);
            }
        }

        if(Input.GetKey(KeyCode.S) && !isDead)
        {
            direction = new Vector2(0, -1);
            transform.Translate(direction * movingSpeed * Time.deltaTime);

            //Animation
            moveAnimation.SetBool("Down", true);

            //Pollen
            if(hasPollen && !pollen.transform.GetChild(2).gameObject.activeSelf)
            {
                pollen.transform.GetChild(0).gameObject.SetActive(false);
                pollen.transform.GetChild(1).gameObject.SetActive(false);
                pollen.transform.GetChild(2).gameObject.SetActive(true);
                pollen.transform.GetChild(3).gameObject.SetActive(false);
            }
        }

        if(Input.GetKey(KeyCode.D) && !isDead)
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

            //Pollen
            if(hasPollen && !pollen.transform.GetChild(3).gameObject.activeSelf)
            {
                pollen.transform.GetChild(0).gameObject.SetActive(false);
                pollen.transform.GetChild(1).gameObject.SetActive(false);
                pollen.transform.GetChild(2).gameObject.SetActive(false);
                pollen.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Wasp")
        {
            //Disable Player Controll
            isDead = true;

            //Exit animations
            moveAnimation.enabled = false;

            //Change Sprite
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = deadSprite;

            //Enable Gravitiy, so bee falls down
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            
            //Enable Rotation of physics simulation
            gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;

            //GameOver Screen
            gameOver.SetActive(true);
            gameOver.transform.GetChild(0).GetComponent<TMP_Text>().text = "Game Over";
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Flower")
        {
            if(other.transform.parent.GetComponent<FlowerManager>().hasPollen && !hasPollen)
            {
                //Activate Pollen Sprite
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

                //Bee now has Pollen
                hasPollen = true;

                //Flower no longer has Pollen
                other.transform.parent.GetComponent<FlowerManager>().hasPollen = false;
            }
        }

        if(other.tag == "Bee Nest")
        {
            //Add 1 to pollenCounter variable in BeeNestManager
            if(hasPollen)
            {
                //Count Pollen
                other.transform.parent.GetComponent<BeeNestManager>().pollenAmount += 1;

                //deactivate pollen sprite
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

                //Bee no longer has pollen
                hasPollen = false;
            }
        }
    }
}
