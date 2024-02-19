using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bee;

    private Vector3 newPosition;    //Position of the camera

    private Vector3 targetPosition;

    [SerializeField]
    private float damping = 1.5f;              //Damping of camera movement on y axis

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Target position needs to be calculated taking the Z axis into account. The camera and the bee cannot be at the same z position, otherwise the camera cannot image the bee. 
        targetPosition = new Vector3(bee.transform.position.x, bee.transform.position.y, transform.position.z);
        newPosition = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
        transform.position = newPosition;
    }
}
