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
        Scene currentScene = SceneManager.GetActiveScene();//現在のシーンを読み込む
        _sceneIndex = currentScene.buildIndex;//現在のシーンのビルドインデックスを取得
        SceneManager.LoadScene(_sceneIndex + 1);//取得した値のシーンを読み込む
    }
}
