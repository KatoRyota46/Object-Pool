using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    ObjectPoolCtrl _objectPool = default;
    public GameObject _bullet = default;
    public GameObject _bulletFirePosition = default;
    private float _cooldownTimer = 0;
    private float _timeInterval = 0.2f;

    private void Update()
    {
        Fire();
        _cooldownTimer -= Time.deltaTime;
    }

    private void Fire()
    {
        if (_cooldownTimer <= 0)
        {
            GameObject obj = _objectPool.GetPooledObject();
            if(obj == null)
            {
                return;
            }
            obj.transform.position = _bulletFirePosition.transform.position;
            obj.transform.rotation = _bulletFirePosition.transform.rotation;
            obj.SetActive(true);
            _cooldownTimer = _timeInterval;
        }
    }
}
