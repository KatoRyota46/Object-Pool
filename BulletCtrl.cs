using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [Header("弾の速度")]
    [SerializeField]
    private Vector3 _bulletMove = default;//弾の速度
    private float _bulletDamage = 1;

    public float BulletDamage {
        get => _bulletDamage;
        set => _bulletDamage = value;
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        transform.localPosition+= _bulletMove;
    }

    //弾を発射するときに初期化するための関数
    public void Init(float angle, float speed)
    {
        //ための発射角度をベクトルに変換する
        Vector3 direction = Utils.GetDirection(angle);
        //発射角度と速さから速度を求める
        _bulletMove = direction * speed;
        //弾が進行方向を向くようにする
        Vector3 angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //敵に触れたら非表示
        if (collision.gameObject.tag == "Enemy") {
            this.gameObject.SetActive(false);
        }
    }
}
