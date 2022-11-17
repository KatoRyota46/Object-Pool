using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    public GameObject _bulletObj;

    private void OnBecameInvisible()
    {
        _bulletObj.SetActive(false);
    }
}
