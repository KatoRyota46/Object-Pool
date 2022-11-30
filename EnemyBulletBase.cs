using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    public ObjectPoolCtrl _objectPool;
    public GameObject _enemyBullet = default;
    public GameObject _enemyBulletFirePosition = default;
    private float _cooldownTimer = 0;
    private float _timeInterval = 0.5f;//弾発射ディレイ

    private void Update() {
        Fire();
        _cooldownTimer -= Time.deltaTime;
    }

    private void Fire() {
        if (_cooldownTimer <= 0) {
            //オブジェクトプール取得
            GameObject obj = _objectPool.GetPooledObject();
            if (obj == null) {
                return;
            }
            //球の発射位置と角度を格納し表示する
            obj.transform.position = _enemyBulletFirePosition.transform.position;
            obj.transform.rotation = _enemyBulletFirePosition.transform.rotation;
            obj.SetActive(true);
            _cooldownTimer = _timeInterval;
        }
    }
}
