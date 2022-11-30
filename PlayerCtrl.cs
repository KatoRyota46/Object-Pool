using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolCtrl _objectPool = default;
    [SerializeField]
    private EnemyCtrl _enemyCtrl;
    public GameObject _bullet = default;//�v���C���[�̒e
    public GameObject _bulletFirePosition = default;//�e�̏o���ʒu
    [SerializeField]
    private float _playerHp = default;//�v���C���[����HP
    [SerializeField]
    private float _playerMaxHp = default;//�v���C���[�ő�HP
    public GameObject _playerSpawn;//�v���C���[�̏o���ʒu
    public EnemyBulletCtrl _enemyBullet;//�G�̒e
    private SpriteRenderer _spriteRenderer;
    private float _cooldownTimer = 0;//�e���ˎ��ԏ�����
    private float _playerSpawnTimer = 0;//�v���C���[�o�����ԏ�����
    private float _playerSpawnInterval = 1f;//�v���C���[�o���Ԋu
    private float _timeInterval = 0.05f;//�e���˃f�B���C
    [Header("�t���b�V�����s���Ԋu")]
    [SerializeField]
    private float _flashInterval = default;//�t���b�V���Ԋu
    [Header("�t���b�V�����s����")]
    [SerializeField]
    private int _flashCount = default;//�t���b�V����
    private BoxCollider2D _boxCollider;

    //�_���[�W���̃X�e�[�^�X
    private enum STATE {
        NOMAL,//�ʏ�
        DAMAGE,//�_���[�W
        INVINCIBLE//���G
    }
    STATE _state;

    private void Awake() {
        //����HP���ő�HP�ɏ���������
        _playerHp = _playerMaxHp;
        //�X�|�[���^�C�}�[���C���^�[�o���ɏ���������
        _playerSpawnTimer = _playerSpawnInterval;
        //�{�b�N�X�R���C�_�[�擾
        _boxCollider = GetComponent<BoxCollider2D>();
        //�X�v���C�g�����_���[�擾
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //���˂Ɉړ�
        Fire();
        //���t���[�����ƂɃN�[���_�E�������炷
        _cooldownTimer -= Time.deltaTime;
        //���t���[�����ƂɃX�|�[���^�C�}�[�����炷
        _playerSpawnTimer -= Time.deltaTime;
        //�X�^�[�^�X���_���[�W�ł���Ȃ牺�̏����𒆒f����
        if (_state == STATE.DAMAGE) {
            return;
        }
        /*
         * �v���C���[��HP��0�ȉ��ł���Ȃ�
         * �v���C���[���\���ɂ��A
         * �v���C���[�̃|�W�V�������X�|�[���|�C���g�ɖ߂��\������
         * HP������������
         */
        if (_playerHp <= 0) {
            this.gameObject.SetActive(false);
            if (_playerSpawnTimer <= 0) {
                transform.position = _playerSpawn.transform.position;
                this.gameObject.SetActive(true);
                _playerHp = _playerMaxHp;
            }
        }
    }
    /// <summary>
    /// ���ˊԊu
    /// </summary>
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
    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <returns></returns>
    private IEnumerator Hit() {
        //���G��
        int invincibleCount = 20;
        //�{�b�N�X�R���C�_�[��L����
        _boxCollider.enabled = false;
        //�����_���[�̐F�����ɂ���
        _spriteRenderer.color = Color.black;

        //�t���b�V���J�E���g�̐��l��������
        for (int i = 0; i < _flashCount; i++) {
            //�t���b�V���C���^�[�o���̐��l�������~�߂�
            yield return new WaitForSeconds(_flashInterval);
            //�{�b�N�X�R���C�_�[�̔�L����
            _spriteRenderer.enabled = false;
            //�t���b�V���C���^�[�o���̐��l�������~�߂�
            yield return new WaitForSeconds(_flashInterval);
            //�{�b�N�X�R���C�_�[�L����
            _spriteRenderer.enabled = true;

            //i�̒l�����G�J�E���g�𒴂�����
            if (i > invincibleCount) {
                //�X�e�[�^�X�𖳓G�ɕύX����
                _state = STATE.INVINCIBLE;
                //�����_���[�̐F��΂ɂ���
                _spriteRenderer.color = Color.green;
            }
        }

        //�X�e�[�^�X��ʏ�ɕύX����
        _state = STATE.NOMAL;
        //�{�b�N�X�R���C�_�[�L����
        _boxCollider.enabled = true;
        //�����_���[�̐F��Ԃɂ���
        _spriteRenderer.color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (_state != STATE.NOMAL) {
                return;
            }
            _playerHp -= _enemyCtrl._enemyHitDamage;
            _state = STATE.DAMAGE;
            StartCoroutine(Hit());
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (_state != STATE.NOMAL) {
                return;
            }
            _playerHp -= _enemyCtrl._enemyHitDamage;
            _state = STATE.DAMAGE;
            StartCoroutine(Hit());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "EnemyBullet") {
            if (_state != STATE.NOMAL) {
                return;
            }
            _playerHp -= _enemyBullet._bulletHitDamage;
            _state = STATE.DAMAGE;
            StartCoroutine(Hit());
        }
    }
}
