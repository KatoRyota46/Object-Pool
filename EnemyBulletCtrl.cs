using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    [Header("弾の移動方向")]
    [SerializeField]
    private Vector3 _enemyBulletMove = default;
    public int _bulletHitDamage = 5;

    public Vector3 EnemyBulletMove {
        get => _enemyBulletMove;
        set => _enemyBulletMove = value;
    }

    private void Update() {
        if (this.gameObject.tag == "EnemyBullet_Beam") {
            _bulletHitDamage = 15;
            if (this.gameObject.tag == "EnemyBullet") {
                _bulletHitDamage = 5;
            }
        }
        transform.position -= EnemyBulletMove;
        if (this.gameObject.transform.position.y <= -30) {
            this.gameObject.SetActive(false);
        }
    }

    //弾を発射するときに初期化するための関数
    public void Init(float angle, float speed) {
        Vector3 direction = Utils.GetDirection(angle);
        EnemyBulletMove = direction * speed;
        Vector3 angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && this.gameObject.tag != "EnemyBullet_Beam") {
            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Bullet" && this.gameObject.tag != "EnemyBullet_Beam") {
            this.gameObject.SetActive(false);
        }
    }
}
