using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region ���[���h�ϐ�
    [SerializeField]
    private ObjectPoolCtrl _objectPool = default;
    [SerializeField]
    private EnemyCtrl _enemyCtrl;
    public GameObject _bullet = default;//�v���C���[�̒e
    [SerializeField]
    private GameObject _bulletFirePosition = default;//�e�̏o���ʒu
    [SerializeField]
    private GameObject _cordlessBulletFirePosition;//�q�@�̒e�̏o���ʒu
    [SerializeField]
    private GameObject _cordlessHandset;//�q�@�̃Q�[���I�u�W�F�N�g
    [SerializeField]
    private BulletCtrl _bulletCtrl;//�v���C���[���̒e
    private int _playerHp = default;//�v���C���[����HP
    private int _playerMaxHp = 100;//�v���C���[�ő�HP
    [SerializeField]
    private GameObject _playerSpawn;//�v���C���[�̏o���ʒu
    public EnemyBulletCtrl _enemyBullet;//�G�̒e
    private SpriteRenderer _spriteRenderer;
    private float _cooldownTimer = 0;//�e���ˎ��ԏ�����
    private float _playerSpawnTimer = 0;//�v���C���[�o�����ԏ�����
    private float _playerSpawnInterval = 1f;//�v���C���[�o���Ԋu
    private float _timeInterval = 0.05f;//�e���˃f�B���C
    private float _shotDelay = 0.4f;
    private int _nextExpBase = 16;//���̃��x���܂łɕK�v�Ȍo���l�̊�{�l
    private int _nextExpInterval = 20;//���̃��x���܂łɕK�v�Ȍo���l�̑����l
    private int _level = 1;//���݃��x��
    private int _playerExp = default;//���݌o���l
    private int _previewNeedExp = default;//�O�̃��x���ɕK�v�������o���l
    private int _needExp = default;//���̃��x���ɕK�v�Ȍo���l
    private int _levelMax = 999;//���x�����
    private int _playerDefence = 0;//�v���C���[�̖h���
    private const int PLAYER_MAX_DEFENCE = 9;//�v���C���[�̖h��͂̍ő�l�̒萔
    private const float ATTACK_BASE_VALUE = 1.1f;//�U���͂̊�{�����l
    private float _attackInterval = 0.5f;//���x���㏸���̍U���͂̑����l
    private bool _isCordlessHandset = false;
    private HeelPoint _heelPoint;
    [SerializeField]
    private float _flashInterval = default;//�t���b�V���Ԋu
    [SerializeField]
    private int _flashCount = default;//�t���b�V����
    private BoxCollider2D _boxCollider;
    private static PlayerCtrl _playerInstance;
    private int _residue = 1;//�v���C���[�c��
    private int _residueCount = 0;
    [SerializeField]
    private GameObject[] _residueIcons;
    #endregion

    //�_���[�W���̃X�e�[�^�X
    private enum STATE {
        NOMAL,//�ʏ�
        DAMAGE,//�_���[�W
        INVINCIBLE//���G
    }
    STATE _state;

    #region �v���p�e�B�Q
    public int Level {
        get => _level;
        set => _level = value;
    }
    public int PlayerExp {
        get => _playerExp;
        set => _playerExp = value;
    }
    public int PreviewNeedExp {
        get => _previewNeedExp;
        set => _previewNeedExp = value;
    }
    public int NeedExp {
        get => _needExp;
        set => _needExp = value;
    }
    public static PlayerCtrl PlayerInstance {
        get => _playerInstance;
        set => _playerInstance = value;
    }
    public int PlayerHp {
        get => _playerHp;
        set => _playerHp = value;
    }
    public int PlayerMaxHp {
        get => _playerMaxHp;
        set => _playerMaxHp = value;
    }

    public int Residue {
        get => _residue;
        set => _residue = value;
    }
    #endregion

    private void Awake() {
        PlayerInstance = this;
        //�������x��
        Level = 1;
        //���̃��x���ɕK�v�Ȍo���l
        NeedExp = GetNeedExp(1);
        //����HP���ő�HP�ɏ���������
        PlayerHp = PlayerMaxHp;
        //�X�|�[���^�C�}�[���C���^�[�o���ɏ���������
        _playerSpawnTimer = _playerSpawnInterval;
        //�{�b�N�X�R���C�_�[�擾
        _boxCollider = GetComponent<BoxCollider2D>();
        //�X�v���C�g�����_���[�擾
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //�q�@���\��
        _cordlessHandset.SetActive(false);
    }

    private void Update()
    {
        transform.localPosition = Utils.ClampPosition(transform.localPosition);
        Vector3 cameraPos = Camera.main.WorldToScreenPoint(transform.position);
        //���˂Ɉړ�
        Fire();
        //���t���[�����ƂɃN�[���_�E�������炷
        _cooldownTimer = _cooldownTimer + 0.01f;
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
        if (PlayerHp <= 0) {
            this.gameObject.SetActive(false);
            if (_playerSpawnTimer <= 0) {
                transform.position = _playerSpawn.transform.position;
                this.gameObject.SetActive(true);
                _state = STATE.NOMAL;
                PlayerHp = PlayerMaxHp;
                _residue--;
                _residueCount++;
                UpdateResidueIcon();
            }
        }
    }
    /// <summary>
    /// ���ˊԊu
    /// </summary>
    private void Fire()
    {
        //�N�[���_�E���ȏ�Ȃ�Δ��˂���
        if (_cooldownTimer >= _shotDelay)
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
            if (_isCordlessHandset) {
                _cordlessHandset.SetActive(true);
                obj = _objectPool.GetPooledObject();
                if (obj == null) {
                    return;
                }
                obj.transform.position = _cordlessBulletFirePosition.transform.position;
                obj.SetActive(true);
            }
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

    /// <summary>
    /// �o���l�����֐�
    /// �N���X�^�����擾�����Ƃ��ɌĂяo�����
    /// </summary>
    /// <param name="exp">���݂�EXP</param>
    public void AddExp(int exp) {
        if (Level == _levelMax) {
            return;
        }
        //�v���C���[�̌o���l�𑝂₷
        PlayerExp += exp;
        //�܂����x���A�b�v�ɕK�v�Ȍo���l�ɑ���Ă��Ȃ��ꍇ�A�����𒆒f����
        if (PlayerExp < NeedExp) {
            return;
        }
        //���x���A�b�v
        Level++;
        //���x���A�b�v�ɕK�v�������o���l���L��
        PreviewNeedExp = NeedExp;
        //���̃��x���A�b�v�ɕK�v�Ȍo���l���v�Z����
        NeedExp = GetNeedExp(Level);
        LevelUpStatus(Random.Range(1,6));
    }

    /// <summary>
    /// �w�肳�ꂽ���x���ɕK�v�Ȍo���l���v�Z����֐�
    /// </summary>
    /// <param name="level">���݂̃��x��</param>
    /// <returns></returns>
    private int GetNeedExp(int level) {
        /*
         * �o���l�Z�o���@
         * ��{�l��16, �����l��21�̏ꍇ
         * ���x��1: 16 + ( 21 * ( 1 - 1 ) * ( 1 - 1 ) ) = 16
         * ���x��2: 16 + ( 22 * ( 2 - 1 ) * ( 2 - 1 ) ) = 38
         * ���x��3: 16 + ( 23 * ( 3 - 1 ) * ( 3 - 1 ) ) = 108
         * ���x��4: 16 + ( 24 * ( 4 - 1 ) * ( 4 - 1 ) ) = 232
         * ���x��5: 16 + ( 25 * ( 5 - 1 ) * ( 5 - 1 ) ) = 416
         */
        _nextExpInterval++;
        return _nextExpBase + (_nextExpInterval * (level - 1) * (level - 1));
    }

    /// <summary>
    /// ���x���A�b�v���̃X�e�[�^�X�㏸
    /// �l�ɂ���ď㏸����X�e�[�^�X�����܂�
    /// </summary>
    /// <param name="randomUp">1�`5�̊ԂŒl�����܂�</param>
    private void LevelUpStatus(int randomUp) {
        switch (randomUp) {
            case 1:
                /*�v���C���[�̒e�̍U���͂��v�Z
                 * ���݂̍U���͂���{�l�Ŋ|���āA�����l�𑫂�
                 */
                _bulletCtrl.BulletDamage = Mathf.FloorToInt(_bulletCtrl.BulletDamage * ATTACK_BASE_VALUE) + _attackInterval;
                _attackInterval++;
                break;
            case 2:
                //�ő�HP����
                PlayerMaxHp = PlayerMaxHp + 10;
                break;
            case 3:
                //�e���˂̊Ԋu����
                if (_shotDelay <= 0.2f) {
                    _shotDelay = _shotDelay - 0.01f;
                }
                break;
            case 4:
                //�v���C���[�̖h��͂��萔����������Ώ��������Ȃ�
                if (_playerDefence >= PLAYER_MAX_DEFENCE) {
                    return;
                }
                _playerDefence++;//�h��͏㏸
                break;
            case 5:
                _isCordlessHandset = true;
                randomUp = Random.Range(1, 5);
                break;
        }
    }

    private void UpdateResidueIcon() {
        for (int i = 0; i < _residueIcons.Length; i++) {
            if (_residueCount <= i) {
                _residueIcons[i].gameObject.SetActive(true);
            } else {
                _residueIcons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (_state != STATE.NOMAL) {
                return;
            }
            if ((_enemyCtrl.EnemyHitDamage - _playerDefence) > 0) {
                PlayerHp -= _enemyCtrl.EnemyHitDamage - _playerDefence;

            } else {
                return;
            }
            _state = STATE.DAMAGE;
            StartCoroutine(Hit());
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (_state != STATE.NOMAL) {
                return;
            }
            if ((_enemyCtrl.EnemyHitDamage - _playerDefence) > 0) {
                PlayerHp -= _enemyCtrl.EnemyHitDamage - _playerDefence;

            } else {
                return;
            }
            _state = STATE.DAMAGE;
            StartCoroutine(Hit());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "EnemyBullet_Beam") {
            if (_state != STATE.NOMAL) {
                return;
            }
            PlayerHp -= _enemyBullet._bulletHitDamage - _playerDefence;
            _state = STATE.DAMAGE;
            StartCoroutine(Hit());
        }

        //�񕜃A�C�e���ɂԂ������Ƃ�
        if (collision.gameObject.tag == "Heel") {
            _heelPoint = GameObject.FindGameObjectWithTag("Heel").GetComponent<HeelPoint>();
            //�v���C���[��HP���ő�HP�Ɠ���������ȏ�ł���Ή��̏������s��Ȃ�
            if (PlayerHp == PlayerMaxHp || PlayerHp >= PlayerMaxHp) {
                return;
            }
            //�񕜗ʕ�������
            PlayerHp += _heelPoint._HeelPoint;
            collision.gameObject.SetActive(false);
        }
    }
}
