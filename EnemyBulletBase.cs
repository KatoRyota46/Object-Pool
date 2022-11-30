using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    public ObjectPoolCtrl _objectPool;
    public GameObject _enemyBullet = default;
    public GameObject _enemyBulletFirePosition = default;
    private float _cooldownTimer = 0;
    private float _timeInterval = 0.5f;//�e���˃f�B���C

    private void Update() {
        Fire();
        _cooldownTimer -= Time.deltaTime;
    }

    private void Fire() {
        if (_cooldownTimer <= 0) {
            //�I�u�W�F�N�g�v�[���擾
            GameObject obj = _objectPool.GetPooledObject();
            if (obj == null) {
                return;
            }
            //���̔��ˈʒu�Ɗp�x���i�[���\������
            obj.transform.position = _enemyBulletFirePosition.transform.position;
            obj.transform.rotation = _enemyBulletFirePosition.transform.rotation;
            obj.SetActive(true);
            _cooldownTimer = _timeInterval;
        }
    }
}
