using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    [Header("弾の移動方向")]
    [SerializeField]
    private Vector3 _enemyBulletMove = default;
    public float _bulletHitDamage = 5.5f;

    private void Update() {
        transform.position -= _enemyBulletMove;
    }

    //弾を発射するときに初期化するための関数
    public void Init(float angle, float speed) {
        Vector3 direction = Utils.GetDirection(angle);
        _enemyBulletMove = direction * speed;
        Vector3 angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            this.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Bullet") {
            this.gameObject.SetActive(false);
        }
    }
}
