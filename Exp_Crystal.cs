using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp_Crystal : MonoBehaviour
{
    private int _crystalExp = default;//クリスタルの初期経験値
    public float _deleteTime = default;//削除時間
    private Transform _playerTransform;//追いかける対象のTransform
    [SerializeField]
    private float _crystalMoveSpeed = default;//クリスタルの移動速度
    [SerializeField]
    private float _crystalLimitSpeed = default;//クリスタルの制限速度
    private Rigidbody2D _rigidbody;//クリスタルのRigidbody2D
    private Transform _crystalTrans;//クリスタルのTransform
    private int _randomChengeProbability = default;//生成する経験値のため
    public List<Sprite> _expSprites;//経験値用画像格納

    private void Awake() {
        RandomExpChenge();
        //PayerタグのついたもののTransformを取得
        _playerTransform = GameObject.FindWithTag("Player").transform;
        //Rigidbody取得
        _rigidbody = GetComponent<Rigidbody2D>();
        //クリスタルのtransform取得
        _crystalTrans = GetComponent<Transform>();
    }

    private void Update() {
        //削除時間になったら削除
        //_deleteTime += Time.deltaTime;
        //_deleteTime = _deleteTime * 60;
        //if (_deleteTime >= 10) {
        //    this.gameObject.SetActive(false);
        //    _deleteTime = 0;
        //}
    }

    private void FixedUpdate() {
        //クリスタルから追いかける対象への方向を計算
        Vector3 vector3 = _playerTransform.position - _crystalTrans.position;
        //方向の長さを1に正規化、任意の力をAddForceで加える
        _rigidbody.AddForce(vector3.normalized * _crystalMoveSpeed);
        //X方向の速度を制限
        float speedXTemp = Mathf.Clamp(_rigidbody.velocity.x, -_crystalLimitSpeed, _crystalLimitSpeed);
        //Y方向の速度を制限
        float speedYTemp = Mathf.Clamp(_rigidbody.velocity.y, -_crystalLimitSpeed, _crystalLimitSpeed);
        //実際に制限した値を代入
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
        //出現させる経験値を1～4の間で決める
        _randomChengeProbability = Random.Range(1, 5);
        switch (_randomChengeProbability) {
            case 1:
                //リスト番号0の画像に変更する
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[0];
                //経験値を1～5の間をランダムで決定する
                _crystalExp = Random.Range(1, 6);
                break;
            case 2:
                //リスト番号1の画像に変更する
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[1];
                //経験値を6～10の間をランダムで決定する
                _crystalExp = Random.Range(6, 11);
                break;
            case 3:
                //リスト番号2の画像に変更する
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[2];
                //経験値を7～16の間をランダムで決定する
                _crystalExp = Random.Range(7, 16);
                break;
            case 4:
                //リスト番号3の画像に変更する
                gameObject.GetComponent<SpriteRenderer>().sprite = _expSprites[3];
                //経験値を18～20の間をランダムで決定する
                _crystalExp = Random.Range(18, 21);
                break;
        }
    }
}
