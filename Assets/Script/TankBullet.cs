using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 75.0f;
    private PhotonView pv;
    void Start()
    {
        pv = gameObject.transform.parent.gameObject.transform.parent.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            generateBullet();
        }
    }
    void generateBullet()
    {
        Vector3 xDirection = transform.TransformDirection(Vector3.forward);
        Vector3 velocity = bulletSpeed * xDirection;
        GameObject newBullet = PhotonNetwork.Instantiate("„RTF_Missile_Blue", transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = velocity;
    }
}
