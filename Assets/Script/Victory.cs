using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Victory : MonoBehaviour
{
    public string param;
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}