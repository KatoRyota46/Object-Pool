using UnityEngine;

public class ExplosionSystem : MonoBehaviour
{
    /// <summary>
    /// �����G�t�F�N�g���������ꂽ���ɌĂяo�����֐�
    /// </summary>
    private void Start() {
        // ���o������������폜����
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.gameObject.SetActive(false);
    }
}
