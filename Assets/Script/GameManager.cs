using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    private PhotonView pv;
    public Dictionary<Player, bool> alivePlayer;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            InitGame();
        }
        pv = GetComponent<PhotonView>();
    }
    public void InitGame()
    {
        alivePlayer = new Dictionary<Player, bool>();
        foreach (var PlayerKeyValue in PhotonNetwork.CurrentRoom.Players)
        {
            alivePlayer[PlayerKeyValue.Value] = true;
        }
        float initX = Random.Range(-30.0f, 30.0f);
        float initZ = Random.Range(-30.0f, 30.0f);
        PhotonNetwork.Instantiate("TankFree_Blue", new Vector3(initX, 2.0f, initZ), Quaternion.identity);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        alivePlayer[newPlayer] = true;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (alivePlayer.ContainsKey(otherPlayer))
        {
            alivePlayer.Remove(otherPlayer);
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void CallRPCplayerDead()
    {
        pv.RPC("RPCplayerDead", RpcTarget.All);
    }
    [PunRPC]
    void RPCplayerDead(PhotonMessageInfo info)
    {
        if (alivePlayer.ContainsKey(info.Sender))
        {
            alivePlayer[info.Sender] = false;
        }
        if (CheckGameOver())
        {
            GameObject.Find("winner").GetComponent<Victory>().param = findWinner();
            if (PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene("resultScene");
            }
        }
    }
    bool CheckGameOver()
    {
        int AliveCount = 0;
        foreach (var keyValue in alivePlayer)
        {
            //如果活著
            if (keyValue.Value)
            {
                AliveCount++;
            }
        }
        return AliveCount <= 1;
    }
    string findWinner()
    {
        foreach (var keyValue in alivePlayer)
        {
            if (keyValue.Value)
            {
                return keyValue.Key.NickName;
            }
        }
        return "沒找到";
    }
}
