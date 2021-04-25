using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager :MonoBehaviourPunCallbacks
{

    
    public GameObject informationPanel;
    public TextMeshProUGUI informationText;
    public GameObject searchForGamesButtonGameobject;
    


    // Start is called before the first frame update
    void Start()
    {
        informationPanel.SetActive(true);
        informationText.text = "Search For Games to BATTLE!";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinRandomRoom()
    {
        informationText.text = "Searching for available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGamesButtonGameobject.SetActive(false);


    }


    public void QuitButton()
    {

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();

        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
        


    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
      
        Debug.Log(message);
        informationText.text = message;

        CreateAndJoin();
    }


    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            informationText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for other players...";


        }
        else
        {
            informationText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(informationPanel, 2.0f));
        }

        Debug.Log( " joined to "+ PhotonNetwork.CurrentRoom.Name);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to "+ PhotonNetwork.CurrentRoom.Name+ " Player count "+ PhotonNetwork.CurrentRoom.PlayerCount);
        informationText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(informationPanel, 2.0f));


    }


   public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    void CreateAndJoin()
    {
        //Choose a random room between 0 and 1000
        string roomName = "Room" + Random.Range(0,1000);
        //Set max players
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        //Creating the room passing name and options
        PhotonNetwork.CreateRoom(roomName,options);

    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);

    }
}
