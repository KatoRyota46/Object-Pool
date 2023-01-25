using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolCtrl _objectPool = default;
    [SerializeField]
    private EnemyCtrl _enemyCtrl;
    public GameObject _bullet = default;//プレイヤーの弾
    public GameObject _bulletFirePosition = default;//弾の出現位置
    [SerializeField]
    private float _playerHp = default;//プレイヤー現在HP
    [SerializeField]
    private float _playerMaxHp = default;//プレイヤー最大HP
    public GameObject _playerSpawn;//プレイヤーの出現位置
    public EnemyBulletCtrl _enemyBullet;//敵の弾
    private SpriteRenderer _spriteRenderer;
    private float _cooldownTimer = 0;//弾発射時間初期化
    private float _playerSpawnTimer = 0;//プレイヤー出現時間初期化
    private float _playerSpawnInterval = 1f;//プレイヤー出現間隔
    private float _timeInterval = 0.05f;//弾発射ディレイ
    [Header("フラッシュを行う間隔")]
    [SerializeField]
    private float _flashInterval = default;//フラッシュ間隔
    [Header("フラッシュを行う回数")]
    [SerializeField]
    private int _flashCount = default;//フラッシュ回数
    private BoxCollider2D _boxCollider;

    //ダメージ時のステータス
    private enum STATE {
        NOMAL,//通常
        DAMAGE,//ダメージ
        INVINCIBLE//無敵
    }
    STATE _state;

    private void Awake() {
        //現在HPを最大HPに初期化する
        _playerHp = _playerMaxHp;
        //スポーンタイマーをインターバルに初期化する
        _playerSpawnTimer = _playerSpawnInterval;
        //ボックスコライダー取得
        _boxCollider = GetComponent<BoxCollider2D>();
        //スプライトレンダラー取得
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //発射に移動
        Fire();
        //毎フレームごとにクールダウンを減らす
        _cooldownTimer -= Time.deltaTime;
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
    /// 発射間隔
    /// </summary>
    private void Fire()
    {
        //クールダウン以上ならば発射する
        if (_cooldownTimer <= 0)
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
