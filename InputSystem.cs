using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour {
    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField]
    private GameObject _playerObject;
    private float _playerMoveSpeed = 0.05f;

    //�ړ����������p�ϐ�
    private Vector2 _playerPos;
    //��������
    private readonly float _playerPosXClamp = 10.735f;
    //�c������
    private readonly float _playerPosYClamp = 4.5f;

    // Update is called once per frame
    void Update() {
        //����
        float horizontal = Input.GetAxis("Horizontal");
        //�c��
        float vertical = Input.GetAxis("Vertical");
        //�v���C���[�|�W�V����
        Vector2 playerPosition = transform.position;
        //#region �Q�[���p�b�h����
        //// �Q�[���p�b�h���ڑ�����Ă��Ȃ���null�ɂȂ�B
        //if (Gamepad.current == null)
        //{
        //    return;
        //}
        //#endregion

        #region �L�[�{�[�h����
        //�E�ړ�
        if (horizontal > 0) {
            playerPosition.x += _playerMoveSpeed;
        }
        //���ړ�
        else if (horizontal < 0) {
            playerPosition.x -= _playerMoveSpeed;
        }
        //��ړ�
        if (vertical > 0) {
            playerPosition.y += _playerMoveSpeed;
        }
        //���ړ�
        else if (vertical < 0) {
            playerPosition.y -= _playerMoveSpeed;
        }
        transform.position = playerPosition;
        #endregion
        MovingRestrictions();
    }

    private void MovingRestrictions() {
        //�ϐ��Ɏ����̍��̈ʒu������
        this._playerPos = transform.position;

        //playerPos�ϐ���x��y�ɐ��������l������
        //playerPos.x�Ƃ����l��-playerPosXClamp�`playerPosXClamp�̊ԂɎ��߂�
        this._playerPos.x = Mathf.Clamp(this._playerPos.x, -this._playerPosXClamp, this._playerPosXClamp);
        this._playerPos.y = Mathf.Clamp(this._playerPos.y, -this._playerPosYClamp, this._playerPosYClamp);

        transform.position = new Vector2(this._playerPos.x, this._playerPos.y);
    }
    
}
