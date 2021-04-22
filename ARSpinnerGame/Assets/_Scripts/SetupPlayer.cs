using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SetupPlayer : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine){
            //Local player being controlled
            transform.GetComponent<MovementController>().enabled = true;
            //Get a handle on local players joystick object
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true); 
        }
        else{
            //Network player disable the movement controller for that player
             transform.GetComponent<MovementController>().enabled = false;
            //Disable control of other players character
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
