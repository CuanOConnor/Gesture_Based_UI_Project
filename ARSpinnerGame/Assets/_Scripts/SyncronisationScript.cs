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
                    Time.fixedDeltaTime);

            rb.rotation =
                Quaternion
                    .RotateTowards(rb.rotation,
                    networkRotation,
                    Time.fixedDeltaTime * 100); //Multiplied by 100 to make change more obvious
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
        }
        else
        {
            //reading data
            //Player game object which exists in the remote players game
            networkPos = (Vector3) stream.ReceiveNext();
            networkRotation = (Quaternion) stream.ReceiveNext();
        }
    }
}
