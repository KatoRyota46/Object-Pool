using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [Header("�e�̑��x")]
    [SerializeField]
    private Vector3 _bulletMove = default;//�e�̑��x
    private float _bulletDamage = 1;

    public float BulletDamage {
        get => _bulletDamage;
        set => _bulletDamage = value;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        transform.localPosition+= _bulletMove;
    }

    //�e�𔭎˂���Ƃ��ɏ��������邽�߂̊֐�
    public void Init(float angle, float speed)
    {
        //���߂̔��ˊp�x���x�N�g���ɕϊ�����
        Vector3 direction = Utils.GetDirection(angle);
        //���ˊp�x�Ƒ������瑬�x�����߂�
        _bulletMove = direction * speed;
        //�e���i�s�����������悤�ɂ���
        Vector3 angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //�G�ɐG�ꂽ���\��
        if (collision.gameObject.tag == "Enemy") {
            this.gameObject.SetActive(false);
        }
    }
}
