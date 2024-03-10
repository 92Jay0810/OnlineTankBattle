using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Text;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text RoomNameText;
    [SerializeField] Text playerNameList;
    [SerializeField] Button startGameButton;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
            updatePlayerList();
        }
        //房主才可以開啟遊戲
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
    }
    //更換房主時，按鈕也要更新
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
    }
    public void updatePlayerList()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var PlayerKeyValue in PhotonNetwork.CurrentRoom.Players)
        {
            sb.AppendLine("  -> " + PlayerKeyValue.Value.NickName);
        }
        playerNameList.text = sb.ToString();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        updatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updatePlayerList();
    }
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("playScene");
    }
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
