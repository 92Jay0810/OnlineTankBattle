using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviourPunCallbacks
{
    public void OnclickStartButton()
    {
        //����笖[�厩���؏�i
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("start");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected");
        SceneManager.LoadScene("LobbyScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
