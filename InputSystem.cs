using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour {
    [Header("プレイヤーオブジェクト")]
    [SerializeField]
    private GameObject _playerObject;
    private float _playerMoveSpeed = 0.05f;

    //移動制限処理用変数
    private Vector2 _playerPos;
    //横軸制限
    private readonly float _playerPosXClamp = 10.735f;
    //縦軸制限
    private readonly float _playerPosYClamp = 4.5f;

    // Update is called once per frame
    void Update() {
        //横軸
        float horizontal = Input.GetAxis("Horizontal");
        //縦軸
        float vertical = Input.GetAxis("Vertical");
        //プレイヤーポジション
        Vector2 playerPosition = transform.position;
        //#region ゲームパッド操作
        //// ゲームパッドが接続されていないとnullになる。
        //if (Gamepad.current == null)
        //{
        //    return;
        //}
        //#endregion

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
        transform.position = playerPosition;
        #endregion
        MovingRestrictions();
    }

    private void MovingRestrictions() {
        //変数に自分の今の位置を入れる
        this._playerPos = transform.position;

        //playerPos変数のxとyに制限した値を入れる
        //playerPos.xという値を-playerPosXClamp～playerPosXClampの間に収める
        this._playerPos.x = Mathf.Clamp(this._playerPos.x, -this._playerPosXClamp, this._playerPosXClamp);
        this._playerPos.y = Mathf.Clamp(this._playerPos.y, -this._playerPosYClamp, this._playerPosYClamp);

        transform.position = new Vector2(this._playerPos.x, this._playerPos.y);
    }
    
}
