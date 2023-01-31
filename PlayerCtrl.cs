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
    private CircleCollider2D _circleCollider;
    private static PlayerCtrl _playerInstance;
    private int _residue = 3;//�v���C���[�c��
    private int _residueCount = 0;//���j��
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
        Level = 1;//�������x��        
        NeedExp = GetNeedExp(1);//���̃��x���ɕK�v�Ȍo���l
        PlayerHp = PlayerMaxHp;//����HP���ő�HP�ɏ���������        
        _playerSpawnTimer = _playerSpawnInterval;//�X�|�[���^�C�}�[���C���^�[�o���ɏ���������        
        _circleCollider = GetComponent<CircleCollider2D>();//�T�[�N���R���C�_�[�擾        
        _spriteRenderer = GetComponent<SpriteRenderer>();//�X�v���C�g�����_���[�擾        
        _cordlessHandset.SetActive(false);//�q�@���\��
    }

    private void Update()
    {
        transform.localPosition = Utils.ClampPosition(transform.localPosition);
        Vector3 cameraPos = Camera.main.WorldToScreenPoint(transform.position);        
        Fire();//���˂Ɉړ�        
        _cooldownTimer = _cooldownTimer + 0.01f;//���t���[�����ƂɃN�[���_�E���𑝂₷        
        _playerSpawnTimer -= Time.deltaTime;//���t���[�����ƂɃX�|�[���^�C�}�[�����炷        
        if (_state == STATE.DAMAGE) {//�X�^�[�^�X���_���[�W�ł���Ȃ牺�̏����𒆒f����
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
                _residue--;//�c������炷
                _residueCount++;//�v���C���[�����j���ꂽ�񐔂𑝂₷
                UpdateResidueIcon();
            }
        }
    }
    /// <summary>
    /// ���ˊԊu
    /// </summary>
    private void Fire()
    {       
        if (_cooldownTimer >= _shotDelay)//�N�[���_�E���ȏ�Ȃ�Δ��˂���
        {            
            GameObject obj = _objectPool.GetPooledObject();//�I�u�W�F�N�g�v�[���擾
            if (obj == null)
            {
                return;
            }
            //���̔��ˈʒu�Ɗp�x���i�[���\������
            obj.transform.position = _bulletFirePosition.transform.position;
            obj.transform.rotation = _bulletFirePosition.transform.rotation;
            obj.SetActive(true);
            _cooldownTimer = _timeInterval;//�N�[���_�E���̃��Z�b�g
            if (_isCordlessHandset) {//���x���A�b�v���Ɏq�@�̒ǉ����I�����ꂽ��
                _cordlessHandset.SetActive(true);//�q�@�̕\��
                obj = _objectPool.GetPooledObject();//�I�u�W�F�N�g�v�[���擾
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
        int invincibleCount = 20;//���G�񐔂�ݒ�       
        _circleCollider.enabled = false;//�{�b�N�X�R���C�_�[��L����        
        _spriteRenderer.color = Color.black;//�����_���[�̐F�����ɂ���
        
        for (int i = 0; i < _flashCount; i++) {//�t���b�V���J�E���g�̐��l��������            
            yield return new WaitForSeconds(_flashInterval);//�t���b�V���C���^�[�o���̐��l�������~�߂�            
            _spriteRenderer.enabled = false;//�{�b�N�X�R���C�_�[�̔�L����            
            yield return new WaitForSeconds(_flashInterval);//�t���b�V���C���^�[�o���̐��l�������~�߂�            
            _spriteRenderer.enabled = true;//�{�b�N�X�R���C�_�[�L����

            if (i > invincibleCount) {//i�̒l�����G�J�E���g�𒴂�����                
                _state = STATE.INVINCIBLE;//�X�e�[�^�X�𖳓G�ɕύX����                
                _spriteRenderer.color = Color.green;//�����_���[�̐F��΂ɂ���
            }
        }        
        _state = STATE.NOMAL;//�X�e�[�^�X��ʏ�ɕύX����        
        _circleCollider.enabled = true;//�{�b�N�X�R���C�_�[�L����        
        _spriteRenderer.color = Color.white;//�����_���[�̐F��Ԃɂ���
    }

    /// <summary>
    /// �o���l�����֐�
    /// �N���X�^�����擾�����Ƃ��ɌĂяo�����
    /// </summary>
    /// <param name="exp">���݂�EXP</param>
    public void AddExp(int exp) {
        //���݃��x���ƍő僌�x���������ł���΃��x���A�b�v�̏��������Ȃ�
        if (Level == _levelMax) {
            return;
        }        
        PlayerExp += exp;//�v���C���[�̌o���l�𑝂₷

        //�܂����x���A�b�v�ɕK�v�Ȍo���l�ɑ���Ă��Ȃ��ꍇ�A�����𒆒f����
        if (PlayerExp < NeedExp) {
            return;
        }        
        Level++;//���x���A�b�v        
        PreviewNeedExp = NeedExp;//���x���A�b�v�ɕK�v�������o���l���L��        
        NeedExp = GetNeedExp(Level);//���̃��x���A�b�v�ɕK�v�Ȍo���l���v�Z����
        LevelUpStatus(Random.Range(1,7));//�㏸������X�e�[�^�X�����߂�
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
    /// <param name="randomUp">1�`6�̊ԂŒl�����܂�</param>
    private void LevelUpStatus(int randomUp) {
        switch (randomUp) {
            case 1:
                /*�v���C���[�̒e�̍U���͂��v�Z
                 * ���݂̍U���͂���{�l�Ŋ|���āA�����l�𑫂�
                 */
                _bulletCtrl.BulletDamage = Mathf.FloorToInt(_bulletCtrl.BulletDamage * ATTACK_BASE_VALUE) + _attackInterval;
                _attackInterval++;
                Debug.Log("�e�̃_���[�W��" + _bulletCtrl.BulletDamage);
                break;
            case 2:                
                PlayerMaxHp = PlayerMaxHp + 10;//�ő�HP����
                Debug.Log("�v���C���[�̍ő�HP��" + PlayerMaxHp);
                break;
            case 3:                
                if (_shotDelay >= 0.05f) {//�e���˂̊Ԋu����
                    _shotDelay = _shotDelay - 0.02f;
                }
                Debug.Log("�e���˂̊Ԋu��" + _shotDelay);
                break;
            case 4:
                //�v���C���[�̖h��͂��萔����������Ώ��������Ȃ�
                if (_playerDefence >= PLAYER_MAX_DEFENCE) {
                    return;
                }
                _playerDefence++;//�h��͏㏸
                Debug.Log("�v���C���[�̖h��͂�" + _playerDefence);
                break;
            case 5:
                _playerHp += _playerMaxHp / 2;//HP�̍ő�l�̔�����
                if (_playerHp >= _playerMaxHp) {//�񕜂������ʌ���HP���ő�HP�ȏ�ɂȂ��
                    _playerHp = _playerMaxHp;//����HP���ő�HP�ɂ���
                }
                Debug.Log("�v���C���[��HP" + _playerHp);
                break;
            case 6:
                _isCordlessHandset = true;//�q�@�ǉ��p�̃t���O���I���ɂ���
                randomUp = Random.Range(1, 6);//�����_���̃����W��1�`5�Ɍ���������
                break;
        }
    }

    private void UpdateResidueIcon() {
        for (int i = 0; i < _residueIcons.Length; i++) {//i�̒l���c���\�����Ă���A�C�R���̕�������
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
