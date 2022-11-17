using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolCtrl _objectPool = default;
    public GameObject _bullet = default;
    public GameObject _bulletFirePosition = default;
    private float _cooldownTimer = 0;
    private float _timeInterval = 0.05f;//弾発射ディレイ

    private void Update()
    {
        Fire();
        _cooldownTimer -= Time.deltaTime;
    }
    //発射間隔
    private void Fire()
    {
        //クールダウン以上ならば発射する
        if (_cooldownTimer <= 0)
        {
            //オブジェクトプール取得
            GameObject obj = _objectPool.GetPooledObject();
            if(obj == null)
            {
                return;
            }
            //球の発射位置と角度を格納し表示する
            obj.transform.position = _bulletFirePosition.transform.position;
            obj.transform.rotation = _bulletFirePosition.transform.rotation;
            obj.SetActive(true);
            _cooldownTimer = _timeInterval;
        }
    }
}
