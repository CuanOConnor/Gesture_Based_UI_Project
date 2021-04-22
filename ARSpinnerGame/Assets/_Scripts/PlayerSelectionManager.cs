using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;

    public Button button_next;

    public Button button_last;

    public int playerSelection;

    public GameObject[] spinners;

    public TextMeshProUGUI playerType;
    public GameObject ui_Selection;
    public GameObject ui_AfterSelection;


    // Start is called before the first frame update
    void Start()
    {
       
        playerSelection = 0;
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NextPlayer()
    {
        playerSelection += 1;

        if (playerSelection >= spinners.Length)
        {
            //end of array
            playerSelection = 0;
        }
        Debug.Log (playerSelection);
        button_next.enabled = false;
        button_last.enabled = false;

        //1 second to make turn between players
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

        if (playerSelection == 0 || playerSelection == 1)
        {
            //The type of player is an attacking type
            playerType.text = "Attacker";
        }
        else
        {
            //The type of player is an defending type
            playerType.text = "Defender";
        }
    }

    public void LastPlayer()
    {
        playerSelection -= 1;
        if (playerSelection < 0)
        {
            //end of array
            playerSelection = spinners.Length - 1;
        }
        Debug.Log (playerSelection);
        button_next.enabled = false;
        button_last.enabled = false;

        //1 second to make turn between players
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));
        if (playerSelection == 0 || playerSelection == 1)
        {
            //The type of player is an attacking type
            playerType.text = "Attacker";
        }
        else
        {
            //The type of player is an defending type
            playerType.text = "Defender";
        }
    }

    public void SelectButton()
    {
        ui_Selection.SetActive(false);
        ui_AfterSelection.SetActive(true);
        //Set a custom properties to players
        ExitGames.Client.Photon.Hashtable selectionProp =
            new ExitGames.Client.Photon.Hashtable {
                {
                    MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER,
                    playerSelection
                }
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties (selectionProp);

    }

    public void ReSelectButton(){
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);
    }

    public void BattleButton(){
        SceneLoader.Instance.LoadScene("Scene_Gameplay");

    }

    public void BackButton(){
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    IEnumerator
    Rotate(
        Vector3 axis,
        Transform transformToRotate,
        float angle,
        float duration = 1.0f
    )
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation =
            transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation =
                Quaternion
                    .Slerp(originalRotation,
                    finalRotation,
                    elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;
        button_next.enabled = true;
        button_last.enabled = true;
    }
}
