using UnityEngine;

//�ÓI�N���X
public static class Utils{
    // �ړ��\�Ȕ͈�
    public static Vector2 _moveLimit = new Vector2(21.98f, 9.5f);

    // �w�肳�ꂽ�ʒu���ړ��\�Ȕ͈͂Ɏ��߂��l��Ԃ�
    public static Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3
        (
            Mathf.Clamp(position.x, -_moveLimit.x, _moveLimit.x),
            Mathf.Clamp(position.y, -_moveLimit.y, _moveLimit.y),
            0
        );
    }

    public static float GetAngle(Vector2 from, Vector2 to) {
        float dx = to.x - from.x;
        float dy = to.y - from.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

    //�w�肳�ꂽ�p�x(0�`360)���x�N�g���ɕϊ����ĕԂ�
    public static Vector3 GetDirection(float angle)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
    }
}
