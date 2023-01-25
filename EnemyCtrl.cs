using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    private float _enemyMaxHp = 7;//�G�̍ő�HP
    private float _enemyHp = default;//�G�̌���HP
    [SerializeField]
    private BulletCtrl _bulletCtrl = default;//�v���C���[���̒e
    [SerializeField]
    private ObjectPoolCtrl _objectPool;//�I�u�W�F�N�g�v�[���i�������g�j
    private ObjectPoolCtrl _objectPoolExp;//�I�u�W�F�N�g�v�[���i�N���X�^���j
    private ObjectPoolCtrl _objectPoolExplosion;//�I�u�W�F�N�g�v�[���i�����j
    private ObjectPoolCtrl _objectPoolHeel;//�I�u�W�F�N�g�v�[���i�񕜃A�C�e���j
    [SerializeField]
    private GameObject _enemyInstante = default;//�o���ꏊ
    private int _itemDrop = default;//�񕜃A�C�e���h���b�v��
    private float _enemyInstantDelay = 1.5f;//�o���Ԋu
    [SerializeField]
    private Vector3 _enemyMoveSpeed = default;//�G�ړ����x
    [SerializeField]
    private int _enemyHitDamage = default;//�G�̓����莞�̃_���[�W
    private Rigidbody2D _rigidBody;
    private bool _isExpInstantiate = false;
    private bool _isExplosionInstantiate = false;
    public int _enemyInstanceCount = 0;
    public bool _isGameClear = false;
    private static EnemyCtrl _enemyInstance;

    [SerializeField]
    private int _enemyDeleteCount = 0;//�G�̌��j���̃J�E���g(wave��switch���Ɏg�p)

    public int EnemyHitDamage {
        get => _enemyHitDamage;
        set => _enemyHitDamage = value;
    }
    public static EnemyCtrl EnemyInstance {
        get => _enemyInstance;
        set => _enemyInstance = value;
    }

    private void Awake() {
        _enemyInstance = this;
        _objectPoolExp = GameObject.FindGameObjectWithTag("O_Exp").GetComponent<ObjectPoolCtrl>();
        _objectPoolExplosion = GameObject.FindGameObjectWithTag("O_Explosion").GetComponent<ObjectPoolCtrl>();
        _objectPoolHeel = GameObject.FindGameObjectWithTag("O_Heel").GetComponent<ObjectPoolCtrl>();
        _enemyHp = _enemyMaxHp;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        EnemyInstantiate();
        transform.position -= _enemyMoveSpeed;
        _enemyInstantDelay -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            int critical = Random.Range(0, 101);
            if (critical >= 90) {
                _enemyHp -= _bulletCtrl.BulletDamage * 2;
            }
            _enemyHp -= _bulletCtrl.BulletDamage;
            Mathf.CeilToInt(_enemyHp);
            if (_enemyHp <= 0) {
                _enemyDeleteCount++;
                HeelItemInstantiate();
                this.gameObject.SetActive(false);
                _isExpInstantiate = true;
                _isExplosionInstantiate = true;
                _enemyHp = _enemyMaxHp;
                ExplosionInstantiate();
                ExpInstantiate();
            }
        }
        if (collision.gameObject.tag == "Box") {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[��
    /// Wave�`��
    /// 100���Ƃɕω�
    /// �G�̐���
    /// </summary>
    private void EnemyInstantiate() {
        GameObject obj = _objectPool.GetPooledObject();
        switch (_enemyDeleteCount) {
            case 1:
                _isGameClear = true;
                break;
            case 100:
                bool isWaveTrigger = false;
                if (!isWaveTrigger) {
                    _enemyMoveSpeed = _enemyMoveSpeed * 1.000001f;
                    isWaveTrigger = true;
                }
                break;
            case 200:
                EnemyHitDamage = EnemyHitDamage + 5;
                break;
            case 300:
                _enemyMaxHp = 50;
                break;
            case 400:
                EnemyHitDamage = EnemyHitDamage * 4;
                break;
            case 500:
                _isGameClear = true;
                break;
        }
        if (obj == null) {
            return;
        }
        obj.transform.position = _enemyInstante.transform.position;
        obj.transform.rotation = _enemyInstante.transform.rotation;
        _enemyHp = _enemyMaxHp;
        obj.SetActive(true);
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[��
    /// �N���X�^���̐���
    /// </summary>
    private void ExpInstantiate() {
        if (_isExpInstantiate) {
            GameObject obj = _objectPoolExp.GetPooledObject();
            if (obj == null) {
                return;
            }
            obj.transform.position = this.gameObject.transform.position;
            obj.transform.rotation = this.gameObject.transform.rotation;
            obj.SetActive(true);
            _isExpInstantiate = false;
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[��
    /// �����̐���
    /// </summary> 
    private void ExplosionInstantiate() {
        if (_isExplosionInstantiate) {
            GameObject obj = _objectPoolExplosion.GetPooledObject();
            if (obj == null) {
                return;
            }
            obj.transform.position = this.gameObject.transform.position;
            obj.transform.rotation = this.gameObject.transform.rotation;
            obj.SetActive(true);
            _isExplosionInstantiate = false;
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[��
    /// �񕜃A�C�e���̐���
    /// </summary>
    private void HeelItemInstantiate() {
        GameObject obj = _objectPoolHeel.GetPooledObject();
        if (obj == null) {
            return;
        }
        //�h���b�v����0�`100�̊ԂŌ��߂�
        _itemDrop = Random.Range(0, 100);
        //�h���b�v��3���ɐݒ�
        if (_itemDrop >= 70) {
            obj.transform.position = this.gameObject.transform.position;
            obj.transform.rotation = this.gameObject.transform.rotation;
            obj.SetActive(true);
        }
    }
}
