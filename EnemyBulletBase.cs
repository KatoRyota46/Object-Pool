using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    private ObjectPoolCtrl _objectPool;
    public GameObject _enemyBullet = default;
    public GameObject _enemyBulletFirePosition = default;
    private float _cooldownTimer = 0;
    private float _timeInterval = 0.5f;//�e���˃f�B���C
    private Vector3 _objScale;
    public ENEMYBULLETTYPE _enemyBulletType;

    public enum ENEMYBULLETTYPE {
        NOMAL,//�ʏ�
        MACHINE_GUN,//�}�V���K���i�A�ˁj
        BEAM//�r�[���i�����^�e�j
    }
    ENEMYBULLETTYPE _state;

    private void Update() {
        _objectPool = GameObject.FindGameObjectWithTag("O_E_Bullet").GetComponent<ObjectPoolCtrl>();
        Fire();
        _cooldownTimer -= Time.deltaTime;
    }

    private void Fire() {
        //�I�u�W�F�N�g�v�[���擾
        GameObject obj = _objectPool.GetPooledObject();
        _state = _enemyBulletType;
        switch (_state) {
            case ENEMYBULLETTYPE.NOMAL:
                _objScale = obj.transform.localScale;
                _objScale.x = 2;
                _objScale.y = 2;
                obj.transform.localScale = _objScale;
                obj.GetComponent<EnemyBulletCtrl>().EnemyBulletMove = new Vector3(0, 0.05f, 0);
                obj.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
                obj.tag = "EnemyBullet";
                if (_cooldownTimer <= 0) {
                    if (obj == null) {
                        return;
                    }
                    //���̔��ˈʒu�Ɗp�x���i�[���\������
                    obj.transform.position = _enemyBulletFirePosition.transform.position;
                    obj.transform.rotation = _enemyBulletFirePosition.transform.rotation;
                    obj.SetActive(true);
                    _cooldownTimer = _timeInterval;
                }
                break;
            case ENEMYBULLETTYPE.MACHINE_GUN:
                _objScale = obj.transform.localScale;
                _objScale.x = 2;
                _objScale.y = 2;
                obj.transform.localScale = _objScale;
                obj.GetComponent<EnemyBulletCtrl>().EnemyBulletMove = new Vector3(0, 0.3f, 0);
                obj.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
                obj.tag = "EnemyBullet";
                _timeInterval = 0.25f;
                if (_cooldownTimer <= 0) {
                    if (obj == null) {
                        return;
                    }
                    //���̔��ˈʒu�Ɗp�x���i�[���\������
                    obj.transform.position = _enemyBulletFirePosition.transform.position;
                    obj.transform.rotation = _enemyBulletFirePosition.transform.rotation;
                    obj.SetActive(true);
                    _cooldownTimer = _timeInterval;
                }
                break;
            case ENEMYBULLETTYPE.BEAM:
                _objScale = obj.transform.localScale;
                _objScale.x = 12f;
                _objScale.y = 10;
                obj.transform.localScale = _objScale;
                obj.GetComponent<EnemyBulletCtrl>().EnemyBulletMove = new Vector3(0, 0.02f, 0);
                obj.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
                obj.tag = "EnemyBullet_Beam";
                _timeInterval = 3f;
                if (_cooldownTimer <= 0) {
                    if (obj == null) {
                        return;
                    }
                    //���̔��ˈʒu�Ɗp�x���i�[���\������
                    obj.transform.position = _enemyBulletFirePosition.transform.position;
                    obj.transform.rotation = _enemyBulletFirePosition.transform.rotation;
                    obj.SetActive(true);
                    _cooldownTimer = _timeInterval;
                }
                break;
        } 
    }
}
