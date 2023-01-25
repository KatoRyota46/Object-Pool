using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region ワールド変数
    [SerializeField]
    private ObjectPoolCtrl _objectPool = default;
    [SerializeField]
    private EnemyCtrl _enemyCtrl;
    public GameObject _bullet = default;//プレイヤーの弾
    [SerializeField]
    private GameObject _bulletFirePosition = default;//弾の出現位置
    [SerializeField]
    private GameObject _cordlessBulletFirePosition;//子機の弾の出現位置
    [SerializeField]
    private GameObject _cordlessHandset;//子機のゲームオブジェクト
    [SerializeField]
    private BulletCtrl _bulletCtrl;//プレイヤー側の弾
    private int _playerHp = default;//プレイヤー現在HP
    private int _playerMaxHp = 100;//プレイヤー最大HP
    [SerializeField]
    private GameObject _playerSpawn;//プレイヤーの出現位置
    public EnemyBulletCtrl _enemyBullet;//敵の弾
    private SpriteRenderer _spriteRenderer;
    private float _cooldownTimer = 0;//弾発射時間初期化
    private float _playerSpawnTimer = 0;//プレイヤー出現時間初期化
    private float _playerSpawnInterval = 1f;//プレイヤー出現間隔
    private float _timeInterval = 0.05f;//弾発射ディレイ
    private float _shotDelay = 0.4f;
    private int _nextExpBase = 16;//次のレベルまでに必要な経験値の基本値
    private int _nextExpInterval = 20;//次のレベルまでに必要な経験値の増加値
    private int _level = 1;//現在レベル
    private int _playerExp = default;//現在経験値
    private int _previewNeedExp = default;//前のレベルに必要だった経験値
    private int _needExp = default;//次のレベルに必要な経験値
    private int _levelMax = 999;//レベル上限
    private int _playerDefence = 0;//プレイヤーの防御力
    private const int PLAYER_MAX_DEFENCE = 9;//プレイヤーの防御力の最大値の定数
    private const float ATTACK_BASE_VALUE = 1.1f;//攻撃力の基本増加値
    private float _attackInterval = 0.5f;//レベル上昇時の攻撃力の増加値
    private bool _isCordlessHandset = false;
    private HeelPoint _heelPoint;
    [SerializeField]
    private float _flashInterval = default;//フラッシュ間隔
    [SerializeField]
    private int _flashCount = default;//フラッシュ回数
    private BoxCollider2D _boxCollider;
    private static PlayerCtrl _playerInstance;
    private int _residue = 1;//プレイヤー残基
    private int _residueCount = 0;
    [SerializeField]
    private GameObject[] _residueIcons;
    #endregion

    //ダメージ時のステータス
    private enum STATE {
        NOMAL,//通常
        DAMAGE,//ダメージ
        INVINCIBLE//無敵
    }
    STATE _state;

    #region プロパティ群
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
        //初期レベル
        Level = 1;
        //次のレベルに必要な経験値
        NeedExp = GetNeedExp(1);
        //現在HPを最大HPに初期化する
        PlayerHp = PlayerMaxHp;
        //スポーンタイマーをインターバルに初期化する
        _playerSpawnTimer = _playerSpawnInterval;
        //ボックスコライダー取得
        _boxCollider = GetComponent<BoxCollider2D>();
        //スプライトレンダラー取得
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //子機を非表示
        _cordlessHandset.SetActive(false);
    }

    private void Update()
    {
        transform.localPosition = Utils.ClampPosition(transform.localPosition);
        Vector3 cameraPos = Camera.main.WorldToScreenPoint(transform.position);
        //発射に移動
        Fire();
        //毎フレームごとにクールダウンを減らす
        _cooldownTimer = _cooldownTimer + 0.01f;
        //毎フレームごとにスポーンタイマーを減らす
        _playerSpawnTimer -= Time.deltaTime;
        //スタータスがダメージであるなら下の処理を中断する
        if (_state == STATE.DAMAGE) {
            return;
        }
        /*
         * プレイヤーのHPが0以下であるなら
         * プレイヤーを非表示にし、
         * プレイヤーのポジションをスポーンポイントに戻し表示して
         * HPを初期化する
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
    /// 発射間隔
    /// </summary>
    private void Fire()
    {
        //クールダウン以上ならば発射する
        if (_cooldownTimer >= _shotDelay)
        {
            //オブジェクトプール取得
            GameObject obj = _objectPool.GetPooledObject();
            if(obj == null)
            {
                return;
            }
            //球の発射位置と角度を格納し表示する
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
    /// ダメージ処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Hit() {
        //無敵回数
        int invincibleCount = 20;
        //ボックスコライダー非有効化
        _boxCollider.enabled = false;
        //レンダラーの色を黒にする
        _spriteRenderer.color = Color.black;

        //フラッシュカウントの数値分だけ回す
        for (int i = 0; i < _flashCount; i++) {
            //フラッシュインターバルの数値分だけ止める
            yield return new WaitForSeconds(_flashInterval);
            //ボックスコライダーの非有効化
            _spriteRenderer.enabled = false;
            //フラッシュインターバルの数値分だけ止める
            yield return new WaitForSeconds(_flashInterval);
            //ボックスコライダー有効化
            _spriteRenderer.enabled = true;

            //iの値が無敵カウントを超えたら
            if (i > invincibleCount) {
                //ステータスを無敵に変更する
                _state = STATE.INVINCIBLE;
                //レンダラーの色を緑にする
                _spriteRenderer.color = Color.green;
            }
        }

        //ステータスを通常に変更する
        _state = STATE.NOMAL;
        //ボックスコライダー有効化
        _boxCollider.enabled = true;
        //レンダラーの色を赤にする
        _spriteRenderer.color = Color.red;
    }

    /// <summary>
    /// 経験値増加関数
    /// クリスタルを取得したときに呼び出される
    /// </summary>
    /// <param name="exp">現在のEXP</param>
    public void AddExp(int exp) {
        if (Level == _levelMax) {
            return;
        }
        //プレイヤーの経験値を増やす
        PlayerExp += exp;
        //まだレベルアップに必要な経験値に足りていない場合、処理を中断する
        if (PlayerExp < NeedExp) {
            return;
        }
        //レベルアップ
        Level++;
        //レベルアップに必要だった経験値を記憶
        PreviewNeedExp = NeedExp;
        //次のレベルアップに必要な経験値を計算する
        NeedExp = GetNeedExp(Level);
        LevelUpStatus(Random.Range(1,6));
    }

    /// <summary>
    /// 指定されたレベルに必要な経験値を計算する関数
    /// </summary>
    /// <param name="level">現在のレベル</param>
    /// <returns></returns>
    private int GetNeedExp(int level) {
        /*
         * 経験値算出方法
         * 基本値が16, 増加値が21の場合
         * レベル1: 16 + ( 21 * ( 1 - 1 ) * ( 1 - 1 ) ) = 16
         * レベル2: 16 + ( 22 * ( 2 - 1 ) * ( 2 - 1 ) ) = 38
         * レベル3: 16 + ( 23 * ( 3 - 1 ) * ( 3 - 1 ) ) = 108
         * レベル4: 16 + ( 24 * ( 4 - 1 ) * ( 4 - 1 ) ) = 232
         * レベル5: 16 + ( 25 * ( 5 - 1 ) * ( 5 - 1 ) ) = 416
         */
        _nextExpInterval++;
        return _nextExpBase + (_nextExpInterval * (level - 1) * (level - 1));
    }

    /// <summary>
    /// レベルアップ時のステータス上昇
    /// 値によって上昇するステータスが決まる
    /// </summary>
    /// <param name="randomUp">1～5の間で値が決まる</param>
    private void LevelUpStatus(int randomUp) {
        switch (randomUp) {
            case 1:
                /*プレイヤーの弾の攻撃力を計算
                 * 現在の攻撃力を基本値で掛けて、増加値を足す
                 */
                _bulletCtrl.BulletDamage = Mathf.FloorToInt(_bulletCtrl.BulletDamage * ATTACK_BASE_VALUE) + _attackInterval;
                _attackInterval++;
                break;
            case 2:
                //最大HP増加
                PlayerMaxHp = PlayerMaxHp + 10;
                break;
            case 3:
                //弾発射の間隔強化
                if (_shotDelay <= 0.2f) {
                    _shotDelay = _shotDelay - 0.01f;
                }
                break;
            case 4:
                //プレイヤーの防御力が定数よりも超えれば処理をしない
                if (_playerDefence >= PLAYER_MAX_DEFENCE) {
                    return;
                }
                _playerDefence++;//防御力上昇
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

        //回復アイテムにぶつかったとき
        if (collision.gameObject.tag == "Heel") {
            _heelPoint = GameObject.FindGameObjectWithTag("Heel").GetComponent<HeelPoint>();
            //プレイヤーのHPが最大HPと同等かそれ以上であれば下の処理を行わない
            if (PlayerHp == PlayerMaxHp || PlayerHp >= PlayerMaxHp) {
                return;
            }
            //回復量分だけ回復
            PlayerHp += _heelPoint._HeelPoint;
            collision.gameObject.SetActive(false);
        }
    }
}
