using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncronisationScript : MonoBehaviour, IPunObservable
{
    Rigidbody rb;

    PhotonView view;

    Vector3 networkPos;

    Quaternion networkRotation;

    public bool syncVelocity = true;

    public bool syncAngularVelocity = true;

    private float distance;

    private float angle;

    public bool isTelePort = true;

    public float teleport = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        networkPos = new Vector3();
        networkRotation = new Quaternion();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!view.IsMine)
        {
            rb.position =
                Vector3
                    .MoveTowards(rb.position,
                    networkPos,
                    distance * (1.0f / PhotonNetwork.SerializationRate));

            rb.rotation =
                Quaternion
                    .RotateTowards(rb.rotation,
                    networkRotation,
                    angle * (1.0f / PhotonNetwork.SerializationRate)); //Multiplied by 100 to make change more obvious
        }
    }

    public void OnPhotonSerializeView(
        PhotonStream stream,
        PhotonMessageInfo info
    )
    {
        //Stream is a container that sends anecieves data to and from a photon view
        if (stream.IsWriting)
        {
            //Writing data
            //I am controlling this player. Send position and velocity data to the other player
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);

            if (syncVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if (syncAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {
            //reading data
            //Player game object which exists in the remote players game
            networkPos = (Vector3) stream.ReceiveNext();
            networkRotation = (Quaternion) stream.ReceiveNext();

            if (isTelePort)
            {
                if (Vector3.Distance(rb.position, networkPos) > teleport)
                {
                    rb.position = networkPos;
                }
            }

            if (syncVelocity || syncAngularVelocity)
            {
                float lag =
                    Mathf
                        .Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (syncVelocity)
                {
                    rb.velocity = (Vector3) stream.ReceiveNext();

                    networkPos += rb.velocity * lag;
                    distance = Vector3.Distance(rb.position, networkPos);
                }

                if (syncAngularVelocity)
                {
                    rb.angularVelocity = (Vector3) stream.ReceiveNext();

                    networkRotation =
                        Quaternion.Euler(rb.angularVelocity * lag) *
                        networkRotation;

                    angle = Quaternion.Angle(rb.rotation, networkRotation);
                }
            }
        }
    }
}
