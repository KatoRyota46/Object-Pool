using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    private float _enemyMaxHp = 7;//敵の最大HP
    private float _enemyHp = default;//敵の現在HP
    [SerializeField]
    private BulletCtrl _bulletCtrl = default;//プレイヤー側の弾
    [SerializeField]
    private ObjectPoolCtrl _objectPool;//オブジェクトプール（自分自身）
    private ObjectPoolCtrl _objectPoolExp;//オブジェクトプール（クリスタル）
    private ObjectPoolCtrl _objectPoolExplosion;//オブジェクトプール（爆発）
    private ObjectPoolCtrl _objectPoolHeel;//オブジェクトプール（回復アイテム）
    [SerializeField]
    private GameObject _enemyInstante = default;//出現場所
    private int _itemDrop = default;//回復アイテムドロップ率
    private float _enemyInstantDelay = 1.5f;//出現間隔
    [SerializeField]
    private Vector3 _enemyMoveSpeed = default;//敵移動速度
    [SerializeField]
    private int _enemyHitDamage = default;//敵体当たり時のダメージ
    private Rigidbody2D _rigidBody;
    private bool _isExpInstantiate = false;
    private bool _isExplosionInstantiate = false;
    public int _enemyInstanceCount = 0;
    public bool _isGameClear = false;
    private static EnemyCtrl _enemyInstance;

    [SerializeField]
    private int _enemyDeleteCount = 0;//敵の撃破数のカウント(waveのswitch文に使用)

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
    /// オブジェクトプール
    /// Wave形式
    /// 100ごとに変化
    /// 敵の生成
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
    /// オブジェクトプール
    /// クリスタルの生成
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
    /// オブジェクトプール
    /// 爆発の生成
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
    /// オブジェクトプール
    /// 回復アイテムの生成
    /// </summary>
    private void HeelItemInstantiate() {
        GameObject obj = _objectPoolHeel.GetPooledObject();
        if (obj == null) {
            return;
        }
        //ドロップ率を0～100の間で決める
        _itemDrop = Random.Range(0, 100);
        //ドロップ率3割に設定
        if (_itemDrop >= 70) {
            obj.transform.position = this.gameObject.transform.position;
            obj.transform.rotation = this.gameObject.transform.rotation;
            obj.SetActive(true);
        }
    }
}
