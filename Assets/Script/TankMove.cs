using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class TankMove : MonoBehaviourPunCallbacks
{
    [SerializeField] int hp = 3;
    [SerializeField] float moveSpeed = 60.0f;
    [SerializeField] float rotationSpeed = 120.0f;
    [SerializeField] GameObject[] leftWheels;
    [SerializeField] GameObject[] rightWheels;
    [SerializeField] float wheelsRotationSpeed = 200.0f;
    private Rigidbody rb;
    private float moveInput;
    private float rotationInput;
    private PhotonView pv;
    private Camera camera;
    [SerializeField] Image hp_image;
    [SerializeField] Text PlayerName;
    GameManager gameManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pv = gameObject.GetComponent<PhotonView>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (pv.IsMine)
        {
            camera = GetComponentInChildren<Camera>();
            camera.enabled = true;
        }
        else
        {
            camera = GetComponentInChildren<Camera>();
            camera.enabled = false;
        }
        PlayerName.text = pv.Owner.NickName;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            moveInput = Input.GetAxis("Vertical");
            rotationInput = Input.GetAxis("Horizontal");
            rotationWheels(moveInput, rotationInput);
            moveTank(moveInput);
            rotateionTank(rotationInput);
        }
    }

    void moveTank(float input)
    {
        Vector3 moveDirection = transform.forward * input * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }
    void rotateionTank(float input)
    {
        //坦克左右繞y軸旋轉，所有子物件包含輪子也會跟著轉
        float rotation = input * rotationSpeed * Time.deltaTime;
        Quaternion trunRotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        rb.MoveRotation(rb.rotation * trunRotation);
    }
    void rotationWheels(float moveInput, float rotationInput)
    {
        float wheelRotation = moveInput * wheelsRotationSpeed * Time.deltaTime;
        foreach (GameObject wheel in leftWheels)
        {
            if (wheel != null)
            {
                //坦克左右旋轉時，車輪跟著坦克繞y軸旋轉。
                //坦克前後移動時，車輪的轉動速度會受到左右旋轉時的旋轉角度的影響。
                //簡單說明是坦克左右旋轉角越大，前進時，車輪的轉速越慢。
                wheel.transform.Rotate(wheelRotation - rotationInput * wheelsRotationSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
        }
        foreach (GameObject wheel in rightWheels)
        {
            wheel.transform.Rotate(wheelRotation - rotationInput * wheelsRotationSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.tag == "Bullet")
            {
                //不是自己的子彈就要扣血
                if (!other.GetComponent<PhotonView>().IsMine)
                {
                    Hashtable hashtable = new Hashtable();
                    hp -= 1;
                    hashtable.Add("hp", hp);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
                    if (hp <= 0)
                    {
                        PhotonNetwork.Destroy(gameObject);
                        gameManager.CallRPCplayerDead();
                    }
                }
            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == pv.Owner)
        {
            hp = (int)changedProps["hp"];
            Debug.Log(targetPlayer.NickName + " : " + hp.ToString());
            updateHPImage();
        }
    }
    private void updateHPImage()
    {
        float precent = hp / 3.0f;
        hp_image.transform.localScale = new Vector3(precent, hp_image.transform.localScale.y, hp_image.transform.localScale.z);
    }
}

