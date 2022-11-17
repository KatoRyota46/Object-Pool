using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckGroundMove : MonoBehaviour
{
    public Transform _player; // �v���C���[
    public Vector2 _limit; // �w�i�̈ړ��͈�

    // ���t���[���Ăяo�����֐�
    private void Update()
    {
        // �v���C���[�̌��ݒn���擾����
        Vector3 pos = _player.localPosition;

        // ��ʒ[�̈ʒu���擾����
        Vector2 limit = Utils._moveLimit;

        // �v���C���[����ʂ̂ǂ̈ʒu�ɑ��݂���̂����A
        // 0 ���� 1 �̒l�ɒu��������
        float tx = 1 - Mathf.InverseLerp(-limit.x, limit.x, pos.x);
        float ty = 1 - Mathf.InverseLerp(-limit.y, limit.y, pos.y);

        // �v���C���[�̌��ݒn����w�i�̕\���ʒu���Z�o����
        float x = Mathf.Lerp(-_limit.x, _limit.x, tx);
        float y = Mathf.Lerp(-_limit.y, _limit.y, ty);

        // �w�i�̕\���ʒu���X�V����
        transform.localPosition = new Vector3(x, y, 0);
    }
}
