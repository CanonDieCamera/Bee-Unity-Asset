using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //Awake is called before Start
    private void Awake() {
        //Set target Framerate of the Application -> Application can have less, but not more Frames
        Application.targetFrameRate = 30;
    }
}
