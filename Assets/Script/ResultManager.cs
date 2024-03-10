using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text VictoryText;
    void Start()
    {
        string param = GameObject.Find("winner").GetComponent<Victory>().param;
        VictoryText.text = param + " is Winner";
    }

    public void OnClickLeaveLobby()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        Destroy(GameObject.Find("winner"));
        SceneManager.LoadScene("LobbyScene");
    }
}
