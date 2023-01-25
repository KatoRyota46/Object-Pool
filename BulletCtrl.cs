using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [Header("弾の移動方向")]
    [SerializeField]
    private Vector3 _bulletMove = default;
    public int _bulletDamage = 10;

    // Update is called once per frame
    void Update()
    {
        transform.position += _bulletMove;
    }

    //弾を発射するときに初期化するための関数
    public void Init(float angle, float speed)
    {
        Vector3 direction = Utils.GetDirection(angle);
        _bulletMove = direction * speed;
        Vector3 angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            this.gameObject.SetActive(false);
        }
    }
}
