using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    private Button _pressButton;
    private int _sceneIndex = default;

    private void Update() {
        if (Input.anyKey) {
            ChangeScene();   
        }
    }

    public void ChangeScene() {
        Scene currentScene = SceneManager.GetActiveScene();//���݂̃V�[����ǂݍ���
        _sceneIndex = currentScene.buildIndex;//���݂̃V�[���̃r���h�C���f�b�N�X���擾
        SceneManager.LoadScene(_sceneIndex + 1);//�擾�����l�̃V�[����ǂݍ���
    }
}
