using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public int _enemyMaxHp = 10;
    public int _enemyHp = default;
    public BulletCtrl _bulletCtrl = default;
    public ObjectPoolCtrl _objectPool;
    public float _enemyInstantDelay = 0.1f;
    private Rigidbody2D _rd;

    private void Awake() {
        _enemyHp = _enemyMaxHp;
        _rd = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        EnemyInstantiate();
        _enemyInstantDelay -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            _enemyHp -= _bulletCtrl._bulletDamage;
            if (_enemyHp <= 0) {
                this.gameObject.SetActive(false);
            }
        }
    }

    private void EnemyInstantiate() {
        if (_enemyInstantDelay <= 0) {
            GameObject obj = _objectPool.GetPooledObject();
            if (obj == null) {
                return;
            }
            _rd.AddForce(0, -1, 0);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            _enemyInstantDelay = 0;
        }
    }
}
