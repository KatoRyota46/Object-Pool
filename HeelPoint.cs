using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeelPoint : MonoBehaviour
{
    private int _heelPoint = default;//回復量;
    [SerializeField]
    private float _instantTime = default;//とどまれる時間
    private float _deleteTime = 5.5f;//削除時間
    private bool _isEnable = false;

    /// <summary>
    /// 回復量のプロパティ化
    /// </summary>
    public int _HeelPoint {
        get => _heelPoint;
        set => _heelPoint = value;
    }

    private void Awake() {
        _HeelPoint = Random.Range(5, 10);
    }

    private void Update() {
        if (_isEnable) {
            _instantTime += Time.deltaTime;
            if (_instantTime >= _deleteTime) {
                this.gameObject.SetActive(false);
                _instantTime = 0;
                _isEnable = false;
            }
        }
    }

    private void OnEnable() {
        _isEnable = true;
    }
}
