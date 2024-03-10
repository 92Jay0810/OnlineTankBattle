using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RotatePower : MonoBehaviour
{
    [SerializeField] float rotatePower = 90.0f;
    private PhotonView pv;

    void Start()
    {
        pv = gameObject.transform.parent.GetComponent<PhotonView>();
    }
    void Update()
    {
        if (pv.IsMine)
        {
            Control();
        }

    }
    private void Control()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, rotatePower * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, -rotatePower * Time.deltaTime);
        }
    }
}
