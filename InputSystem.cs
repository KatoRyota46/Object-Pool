using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour {
    [Header("�v���C���[�I�u�W�F�N�g")]
    [SerializeField]
    private GameObject _playerObject;
    [Header("�v���C���[�ړ����x")]
    [SerializeField]
    private float _playerMoveSpeed = 0.105f;

    //�ړ����������p�ϐ�
    private Vector2 _playerPos;

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
        //�ᑬ�ړ�
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            _playerMoveSpeed = _playerMoveSpeed / 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            _playerMoveSpeed = 0.105f;
        }
        transform.position = playerPosition;
        #endregion
        transform.localPosition = Utils.ClampPosition(transform.localPosition);
    }
}
