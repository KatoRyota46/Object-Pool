using UnityEngine;

public class ExplosionSystem : MonoBehaviour
{
    /// <summary>
    /// 爆発エフェクトが生成された時に呼び出される関数
    /// </summary>
    private void Start() {
        // 演出が完了したら削除する
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration);
    }
}
