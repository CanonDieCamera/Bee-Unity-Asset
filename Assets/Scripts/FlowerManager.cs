using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    private bool _hasPollen = true; //if this is true, bee can collect pollen
    public bool hasPollen {
        get{
            return _hasPollen;
        }
        set{
            _hasPollen = value;

            if(!_hasPollen && !currentlyRecovering)
            {
                //Change Sprite

                //Recover Pollen
                StartCoroutine(RecoveryPollen());
            }
        }
    }  

    [SerializeField]
    private float pollenRecoveryTime = 10; //Time the flower need to recover pollen in seconds

    private bool currentlyRecovering = false;   //so that the coroutine RecoveryPollen() is not called several times

    //Coroutine that recovers pollen after pollenRecoveryTime seconds
    private IEnumerator RecoveryPollen()
    {
        currentlyRecovering = true;

        yield return new WaitForSeconds(pollenRecoveryTime);

        hasPollen = true;
        currentlyRecovering = false;
    }
}
