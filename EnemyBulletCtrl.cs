using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    [Header("íeÇÃà⁄ìÆï˚å¸")]
    [SerializeField]
    private Vector3 _enemyBulletMove = default;
    private float _enemyBulletDeleteTime = 1.5f;
    private float _enemyBulletDeleteIntarval = 0f;
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
        _enemyBulletDeleteIntarval += _enemyBulletDeleteIntarval + 0.3f;
        if (_enemyBulletDeleteIntarval == _enemyBulletDeleteTime) {
            _enemyBulletDeleteIntarval = 0;
            this.gameObject.SetActive(false);
        }
    }

    //íeÇî≠éÀÇ∑ÇÈÇ∆Ç´Ç…èâä˙âªÇ∑ÇÈÇΩÇﬂÇÃä÷êî
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
