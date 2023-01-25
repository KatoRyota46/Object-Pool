using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp_Crystal : MonoBehaviour
{
    private int _crystalExp = default;//�N���X�^���̏����o���l
    public float _deleteTime = default;//�폜����
    private Transform _playerTransform;//�ǂ�������Ώۂ�Transform
    [SerializeField]
    private float _crystalMoveSpeed = default;//�N���X�^���̈ړ����x
    [SerializeField]
    private float _crystalLimitSpeed = default;//�N���X�^���̐������x
    private Rigidbody2D _rigidbody;//�N���X�^����Rigidbody2D
    private Transform _crystalTrans;//�N���X�^����Transform
    private int _randomChengeProbability = default;//��������o���l�̂���
    public List<Sprite> _expSprites;//�o���l�p�摜�i�[

    private void Awake() {
        RandomExpChenge();
        //Payer�^�O�̂������̂�Transform���擾
        _playerTransform = GameObject.FindWithTag("Player").transform;
        //Rigidbody�擾
        _rigidbody = GetComponent<Rigidbody2D>();
        //�N���X�^����transform�擾
        _crystalTrans = GetComponent<Transform>();
    }

    private void Update() {
        //�폜���ԂɂȂ�����폜
        //_deleteTime += Time.deltaTime;
        //_deleteTime = _deleteTime * 60;
        //if (_deleteTime >= 10) {
        //    this.gameObject.SetActive(false);
        //    _deleteTime = 0;
        //}
    }

    private void FixedUpdate() {
        //�N���X�^������ǂ�������Ώۂւ̕������v�Z
        Vector3 vector3 = _playerTransform.position - _crystalTrans.position;
        //�����̒�����1�ɐ��K���A�C�ӂ̗͂�AddForce�ŉ�����
        _rigidbody.AddForce(vector3.normalized * _crystalMoveSpeed);
        //X�����̑��x�𐧌�
        float speedXTemp = Mathf.Clamp(_rigidbody.velocity.x, -_crystalLimitSpeed, _crystalLimitSpeed);
        //Y�����̑��x�𐧌�
        float speedYTemp = Mathf.Clamp(_rigidbody.velocity.y, -_crystalLimitSpeed, _crystalLimitSpeed);
        //���ۂɐ��������l����
        _rigidbody.velocity = new Vector3(speedXTemp, speedYTemp);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != "Player") {
            return;
        }
        gameObject.SetActive(false);

        PlayerCtrl player = collision.GetComponent<PlayerCtrl>();
        player.AddExp(_crystalExp);
    }

    private void RandomExpChenge() {
        //�o��������o���l��1�`4�̊ԂŌ��߂�
        _randomChengeProbability = Random.Range(1, 5);
        switch (_randomChengeProbability) {
            case 1:
                //���X�g�ԍ�0�̉摜�ɕύX����
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[0];
                //�o���l��1�`5�̊Ԃ������_���Ō��肷��
                _crystalExp = Random.Range(1, 6);
                break;
            case 2:
                //���X�g�ԍ�1�̉摜�ɕύX����
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[1];
                //�o���l��6�`10�̊Ԃ������_���Ō��肷��
                _crystalExp = Random.Range(6, 11);
                break;
            case 3:
                //���X�g�ԍ�2�̉摜�ɕύX����
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[2];
                //�o���l��7�`16�̊Ԃ������_���Ō��肷��
                _crystalExp = Random.Range(7, 16);
                break;
            case 4:
                //���X�g�ԍ�3�̉摜�ɕύX����
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[3];
                //�o���l��18�`20�̊Ԃ������_���Ō��肷��
                _crystalExp = Random.Range(18, 21);
                break;
        }
    }
}
