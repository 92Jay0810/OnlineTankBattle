using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private PhotonView pv;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        pv = gameObject.GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            Destroy(rb);
        }
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank")
        {
            //�s�����ȁC�A�����œ�
            if (!other.GetComponent<PhotonView>().IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
