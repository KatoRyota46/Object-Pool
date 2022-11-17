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
    private float _timeInterval = 0.05f;//�e���˃f�B���C

    private void Update()
    {
        Fire();
        _cooldownTimer -= Time.deltaTime;
    }
    //���ˊԊu
    private void Fire()
    {
        //�N�[���_�E���ȏ�Ȃ�Δ��˂���
        if (_cooldownTimer <= 0)
        {
            //�I�u�W�F�N�g�v�[���擾
            GameObject obj = _objectPool.GetPooledObject();
            if(obj == null)
            {
                return;
            }
            //���̔��ˈʒu�Ɗp�x���i�[���\������
            obj.transform.position = _bulletFirePosition.transform.position;
            obj.transform.rotation = _bulletFirePosition.transform.rotation;
            obj.SetActive(true);
            _cooldownTimer = _timeInterval;
        }
    }
}
