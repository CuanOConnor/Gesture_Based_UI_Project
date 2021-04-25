using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

/*
    Adapted from: 
    Syncronisation
    https://doc.photonengine.com/en-us/pun/current/gameplay/synchronization-and-state
    Lag compensation
    https://doc.photonengine.com/en-us/pun/current/gameplay/lagcompensation

*/
public class SyncronisationScript : MonoBehaviour, IPunObservable
{
    //Rigid body game object
    Rigidbody rb;

    // Instance of photon view
    PhotonView view;

    //Current positon
    Vector3 networkPos;

    //Current rotation
    Quaternion networkRotation;

    //Sync velocity
    public bool syncVelocity = true;

    //Sync anglar velocity velocity
    public bool syncAngularVelocity = true;

    //distance away
    private float distance;

    //angle
    private float angle;

    //Tele port enabled
    public bool isTelePort = true;

    //speed
    public float teleport = 1.0f;

    private GameObject battleArenaGameobject;

    private void Awake()
    {
        //get rigid body
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        networkPos = new Vector3();
        networkRotation = new Quaternion();

        battleArenaGameobject = GameObject.Find("BattleArena");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Using fixed update since a rigid body is being used
    private void FixedUpdate()
    {
        if (!view.IsMine)
        {
            //Rigid body position
            rb.position =
                //If distance is bigger move quicker
                Vector3
                    .MoveTowards(rb.position,
                    networkPos,
                    distance * (1.0f / PhotonNetwork.SerializationRate));

            //Rigid body rotation
            //if angle distance is big rotate quicker
            rb.rotation =
                Quaternion
                    .RotateTowards(rb.rotation,
                    networkRotation,
                    angle * (1.0f / PhotonNetwork.SerializationRate)); //Multiplied by 100 to make change more obvious
        }
    }

    /*
        Photon stream is a container class that sends and receives data to and from a photo beam.
    */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Stream is a container that sends and recieves data to and from a photon view.
        if (stream.IsWriting)
        {
            //Writing data
            //If this photn view is belonging to me.
            //I am controlling this player. Send position and velocity data to the other player
            stream.SendNext(rb.position - battleArenaGameobject.transform.position);
            stream.SendNext(rb.rotation);

            if (syncVelocity)
            {
                //Send velocity
                stream.SendNext(rb.velocity);
            }

            if (syncAngularVelocity)
            {
                //Send angular velocity
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {
            //reading data
            //Player game object which exists in the remote players game
            networkPos = (Vector3)stream.ReceiveNext() + battleArenaGameobject.transform.position; // also send our arena postion
            networkRotation = (Quaternion)stream.ReceiveNext();

            //if teleport is set to true
            if (isTelePort)
            {
                //if the distance is greater than the teleport value
                if (Vector3.Distance(rb.position, networkPos) > teleport)
                {
                    //Rigid body position is = network position
                    rb.position = networkPos;
                }
            }

            if (syncVelocity || syncAngularVelocity)
            {
                // Lag variable
                /*
                    PhotonNetwork.Time is used to syncronise time for all players.
                    It is actually the server time though so it is the sem for each client.
                    info.SentServerTime is the the time it takes to send the data.
                    This is called LAG

                */
                float lag =
                    Mathf
                        .Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (syncVelocity)
                {
                    //Recevive the velocity
                    rb.velocity = (Vector3) stream.ReceiveNext();

                    //current position is rigis bodys velocity * lag
                    networkPos += rb.velocity * lag;

                    //distance set to the Vector3 distance
                    distance = Vector3.Distance(rb.position, networkPos);
                }

                if (syncAngularVelocity)
                {
                    //Recevive the angualr velocity
                    rb.angularVelocity = (Vector3) stream.ReceiveNext();

                    //current position is rigid bodys angualrvelocity * lag
                    networkRotation =
                        Quaternion.Euler(rb.angularVelocity * lag) *
                        networkRotation;

                    //angle
                    angle = Quaternion.Angle(rb.rotation, networkRotation);
                }
            }
        }
    }
}
