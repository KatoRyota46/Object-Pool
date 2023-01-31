using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeelPoint : MonoBehaviour
{
    private int _heelPoint = default;//�񕜗�;
    [SerializeField]
    private float _instantTime = default;//�Ƃǂ܂�鎞��
    private float _deleteTime = 5.5f;//�폜����
    private bool _isEnable = false;

    /// <summary>
    /// �񕜗ʂ̃v���p�e�B��
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
