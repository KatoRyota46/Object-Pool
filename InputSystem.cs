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

    private void Start() {
        // �Q�[���p�b�h���ڑ�����Ă��Ȃ���null�ɂȂ�B
        if (Gamepad.current == null) {
            return;
        }
    }

    // Update is called once per frame
    void Update() {
        //����(�L�[�{�[�h)
        float horizontal = Input.GetAxis("Horizontal");
        //�c��(�L�[�{�[�h)
        float vertical = Input.GetAxis("Vertical");
        //����(�Q�[���p�b�h)
        float joyHori = Input.GetAxis("JoystickHorizontal");
        //�c��(�Q�[���p�b�h)
        float joyVer = Input.GetAxis("JoystickVertical");
        //�v���C���[�|�W�V����
        Vector2 playerPosition = transform.position;
        #region �Q�[���p�b�h����
        //�E�ړ�
        if (joyHori > 0) {
            playerPosition.x += _playerMoveSpeed;
        }
        //���ړ�
        else if (joyHori < 0) {
            playerPosition.x -= _playerMoveSpeed;
        }
        //��ړ�
        if (joyVer > 0) {
            playerPosition.y += _playerMoveSpeed;
        }
        //���ړ�
        else if (joyVer < 0) {
            playerPosition.y -= _playerMoveSpeed;
        }
        //�ᑬ�ړ�
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown("joystick button 5")) {
            _playerMoveSpeed = _playerMoveSpeed / 2;
        }
        if (Input.GetKeyUp("joystick button 4") || Input.GetKeyUp("joystick button 5")) {
            _playerMoveSpeed = 0.105f;
        }
        transform.position = playerPosition;
        #endregion

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
