using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour {
    [Header("プレイヤーオブジェクト")]
    [SerializeField]
    private GameObject _playerObject;
    [Header("プレイヤー移動速度")]
    [SerializeField]
    private float _playerMoveSpeed = 0.105f;

    //移動制限処理用変数
    private Vector2 _playerPos;

    private void Start() {
        // ゲームパッドが接続されていないとnullになる。
        if (Gamepad.current == null) {
            return;
        }
    }

    // Update is called once per frame
    void Update() {
        //横軸(キーボード)
        float horizontal = Input.GetAxis("Horizontal");
        //縦軸(キーボード)
        float vertical = Input.GetAxis("Vertical");
        //横軸(ゲームパッド)
        float joyHori = Input.GetAxis("JoystickHorizontal");
        //縦軸(ゲームパッド)
        float joyVer = Input.GetAxis("JoystickVertical");
        //プレイヤーポジション
        Vector2 playerPosition = transform.position;
        #region ゲームパッド操作
        //右移動
        if (joyHori > 0) {
            playerPosition.x += _playerMoveSpeed;
        }
        //左移動
        else if (joyHori < 0) {
            playerPosition.x -= _playerMoveSpeed;
        }
        //上移動
        if (joyVer > 0) {
            playerPosition.y += _playerMoveSpeed;
        }
        //下移動
        else if (joyVer < 0) {
            playerPosition.y -= _playerMoveSpeed;
        }
        //低速移動
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown("joystick button 5")) {
            _playerMoveSpeed = _playerMoveSpeed / 2;
        }
        if (Input.GetKeyUp("joystick button 4") || Input.GetKeyUp("joystick button 5")) {
            _playerMoveSpeed = 0.105f;
        }
        transform.position = playerPosition;
        #endregion

        #region キーボード操作
        //右移動
        if (horizontal > 0) {
            playerPosition.x += _playerMoveSpeed;
        }
        //左移動
        else if (horizontal < 0) {
            playerPosition.x -= _playerMoveSpeed;
        }
        //上移動
        if (vertical > 0) {
            playerPosition.y += _playerMoveSpeed;
        }
        //下移動
        else if (vertical < 0) {
            playerPosition.y -= _playerMoveSpeed;
        }
        //低速移動
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
