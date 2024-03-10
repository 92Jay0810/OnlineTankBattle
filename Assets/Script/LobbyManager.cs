using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField roomNameInputField;
    [SerializeField] InputField playerNameInputField;
    [SerializeField] Text roomListText;
    private void Start()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            SceneManager.LoadScene("mainMenu");
        }
        else
        {
            //從mainMenu進來的觸發joinLobby，但是從Room進來的，需等待一段時間的連線，才能joinLobby
            if (PhotonNetwork.CurrentLobby == null)
            {
                PhotonNetwork.JoinLobby();
            }
        }
    }
    //room進入lobby，當連線成功就joinLobby
    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby join");
    }
    public void OnClickCreateRoom()
    {
        string roomName = roomNameInputField.text.Trim();
        string playerName = playerNameInputField.text.Trim();
        if (roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.CreateRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            Debug.Log("invalid RoomName or PlayerName");
        }
    }
    public void OnClickJoinRoom()
    {
        string roomName = roomNameInputField.text.Trim();
        string playerName = playerNameInputField.text.Trim();
        if (roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            Debug.Log("invalid RoomName");
        }
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("room joined!");
        SceneManager.LoadScene("RoomScene");
    }
    //每次roomlist更新時，會觸發
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StringBuilder sb = new StringBuilder();
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.PlayerCount > 0)
            {
                sb.AppendLine("       -> " + roomInfo.Name + "                     " + roomInfo.PlayerCount);
            }
        }
        roomListText.text = sb.ToString();
    }
}
