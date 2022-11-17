using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckGroundMove : MonoBehaviour
{
    public Transform _player; // プレイヤー
    public Vector2 _limit; // 背景の移動範囲

    // 毎フレーム呼び出される関数
    private void Update()
    {
        // プレイヤーの現在地を取得する
        Vector3 pos = _player.localPosition;

        // 画面端の位置を取得する
        Vector2 limit = Utils._moveLimit;

        // プレイヤーが画面のどの位置に存在するのかを、
        // 0 から 1 の値に置き換える
        float tx = 1 - Mathf.InverseLerp(-limit.x, limit.x, pos.x);
        float ty = 1 - Mathf.InverseLerp(-limit.y, limit.y, pos.y);

        // プレイヤーの現在地から背景の表示位置を算出する
        float x = Mathf.Lerp(-_limit.x, _limit.x, tx);
        float y = Mathf.Lerp(-_limit.y, _limit.y, ty);

        // 背景の表示位置を更新する
        transform.localPosition = new Vector3(x, y, 0);
    }
}
