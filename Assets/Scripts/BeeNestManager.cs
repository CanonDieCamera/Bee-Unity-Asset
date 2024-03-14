using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeeNestManager : MonoBehaviour
{
    [SerializeField]
    private int pollenAmountWin = 10;
    private int _pollenAmount = 0;

    public int pollenAmount {
        get{
            return _pollenAmount;
        }
        set{
            _pollenAmount = value;

            //Display PollenAmount
            GameObject.FindGameObjectWithTag("Pollen Counter").GetComponent<TMP_Text>().text = _pollenAmount.ToString();

            if(_pollenAmount >= pollenAmountWin)
            {
                //You win!
                youWonScreen.SetActive(true);
            }
        }
    }

    [SerializeField]
    private GameObject youWonScreen;
}
