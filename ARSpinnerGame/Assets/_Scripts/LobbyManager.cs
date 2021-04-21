using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;

    public GameObject ui_LoginGameObject;

    [Header("Lobby UI")]
    public GameObject ui_LobbyGameObject;

    public GameObject ui_3DGameObject;

    [Header("Connection Status UI")]
    public GameObject ui_ConnectionStatusGameObject;
    public Text connectionStatus;
    public bool showConnectionStatus = false;


#region Methods
    // Start is called before the first frame update
    void Start()
    {
        ui_LobbyGameObject.SetActive(false);
        ui_3DGameObject.SetActive(false);
        ui_ConnectionStatusGameObject.SetActive(false);
        ui_LoginGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(showConnectionStatus){
            connectionStatus.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }
        
    }


#endregion



#region UI CALLBACK METHODS
    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ui_LobbyGameObject.SetActive(false);
            ui_3DGameObject.SetActive(false);
            ui_ConnectionStatusGameObject.SetActive(true);
            ui_LoginGameObject.SetActive(false);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or null");
        }
    }

    public void OnQuickMatchButtonClicked(){
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }


#endregion



#region PHOTON Callback Methods
    public override void OnConnected()
    {
        Debug.Log("Connected to server");
    }

    public override void OnConnectedToMaster()
    {
        Debug
            .Log(PhotonNetwork.LocalPlayer.NickName +
            " is connected to photon server");

        ui_LobbyGameObject.SetActive(true);
        ui_3DGameObject.SetActive(true);
        ui_ConnectionStatusGameObject.SetActive(false);
        showConnectionStatus = true;
        ui_LoginGameObject.SetActive(false);

    }


#endregion
}
