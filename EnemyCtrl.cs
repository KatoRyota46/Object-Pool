using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    private int _enemyMaxHp = 10;
    private int _enemyHp = default;
    [SerializeField]
    private BulletCtrl _bulletCtrl = default;
    [SerializeField]
    private ObjectPoolCtrl _objectPool;
    [SerializeField]
    private GameObject _enemyInstante = default;
    private float _enemyInstantDelay = 1.5f;
    [SerializeField]
    private Vector3 _enemyMoveSpeed = default;
    public float _enemyHitDamage = default;
    private float _activeTime = 0f;
    private Rigidbody2D _rigidBody;
    [Header("åoå±íl")]
    [SerializeField]
    public float _exp = default;
    public ExplosionSystem _explosion;

    private void Awake() {
        _enemyHp = _enemyMaxHp;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        //_activeTime += Time.deltaTime;
        //if (_activeTime >= 1.5f) {
        //    return;
        //}
        EnemyInstantiate();
        transform.position -= _enemyMoveSpeed;
        _enemyInstantDelay -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            _enemyHp -= _bulletCtrl._bulletDamage;
            if (_enemyHp <= 0) {
                this.gameObject.SetActive(false);
                Instantiate(_explosion, collision.transform.position, Quaternion.identity);
            }
        }
    }

    private void EnemyInstantiate() {
        if (_enemyInstantDelay <= 0) {
            GameObject obj = _objectPool.GetPooledObject();
            if (obj == null) {
                return;
            }
            obj.transform.position = _enemyInstante.transform.position;
            obj.transform.rotation = _enemyInstante.transform.rotation;
            obj.SetActive(true);
            _enemyInstantDelay = 0;
        }
    }
}
