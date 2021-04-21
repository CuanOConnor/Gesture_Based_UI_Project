using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{   [Header("Login UI")]
    public  InputField playerNameInputField;

    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion



    #region UI CALLBACK METHODS
    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName))
        {
            if(!PhotonNetwork.IsConnected){

                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or null");
        }
    }

    #endregion

    #region PHOTON Callback Methods
    public override void OnConnected(){

        Debug.Log("Connected to server");
    }

    public override void OnConnectedToMaster() {

        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to photon server");
    }

    #endregion
}
