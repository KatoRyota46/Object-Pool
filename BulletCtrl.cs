using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float _bulletMoveSpeed = default;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * _bulletMoveSpeed * Time.deltaTime;
    }
}
