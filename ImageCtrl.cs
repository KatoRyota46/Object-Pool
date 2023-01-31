using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ImageCtrl : MonoBehaviour {
    public Image _hpGauge;//HPゲージ（色付き）
    public Image _expGauge;//EXPゲージ（色付き）
    public Text _levelText;//レベルテキスト
    public Text _textGame;//ゲームオーバーテキスト①
    public Text _textOver;//ゲームオーバーテキスト②
    public Text _textGameClear;//ゲームクリアテキスト
    private PlayerCtrl _player;//プレイヤースクリプト
    private EnemyCtrl _enemy;//敵スクリプト
    private int _imageHp;//ゲージ側の現在HP
    private int _imageMaxHp;//ゲージ側の最大HP
    private int _imageExp;//ゲージ側の現在EXP
    private int _imagePreviewNeedExp;//ゲージ側の前のEXP
    private int _imageNeedExp;//ゲージ側の次のEXP
    private bool _isGameClear = false;//ImageCtrl側のゲームクリアフラグ
    private bool _isGameOver = false;
    private bool _isStatsValue = false;
    private Button _buttonSelect;
    private GameObject _buttonSummary;
    private int _sceneIndex = default;

    private void Start() {
        _buttonSummary = GameObject.FindGameObjectWithTag("ButtonSummary");
        _buttonSelect = GameObject.FindGameObjectWithTag("SelectButton").GetComponent<Button>();
        _buttonSelect.Select();
        _buttonSummary.gameObject.SetActive(false);
    }

    void Update() {
        //プレイヤー取得
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;

        //HPのゲージの表示を更新する
        _imageHp = _player.PlayerHp;
        _imageMaxHp = _player.PlayerMaxHp;
        _hpGauge.fillAmount = (float)_imageHp / _imageMaxHp;

        //EXPのゲージの表示を更新する
        _imageExp = _player.PlayerExp;
        _imagePreviewNeedExp = _player.PreviewNeedExp;
        _imageNeedExp = _player.NeedExp;
        _expGauge.fillAmount = (float)(_imageExp - _imagePreviewNeedExp) / (_imageNeedExp - _imagePreviewNeedExp);

        //レベルの表示を更新する
        _levelText.text = _player.Level.ToString();
        GameState();
    }

    /// <summary>
    /// ゲームリザルト
    /// ゲーム終了時の状況によってフラグのオンオフが決まる
    /// </summary>
    private void GameState() {
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;
        /*Enemy.Ctrl側のゲームクリアフラグがオンかつ、残基が0以上であれば
         * ゲームクリア
         */
        if (_enemy.IsGameClear && _player.Residue >= 0) {
            StartCoroutine(GameStaging(_isStatsValue = true));
            if (_isGameClear) {
                GameTitle();
            }
        }
        /*Enemy.Ctrl側のゲームクリアフラグがオフかつ、残基がなくなれば
         * ゲームオーバー
         */
        else if (_player.Residue == -1 && !_enemy.IsGameClear) {
            StartCoroutine(GameStaging(_isStatsValue = false));
            if (_isGameOver) {
                GameEnd();
            }
        }
    }

    private IEnumerator GameStaging(bool stats) {
        switch (stats) {
            case true:
                Time.timeScale = 0;
                _buttonSummary.gameObject.SetActive(true);
                _textGameClear.gameObject.SetActive(true);
                _textGameClear.GetComponent<RectTransform>().Rotate(0, 0, -1.2f);
                yield return new WaitForSeconds(1);
                _isGameClear = true;
                break;
            case false:
                Time.timeScale = 0;
                _buttonSummary.gameObject.SetActive(true);
                _textGame.gameObject.SetActive(true);
                _textOver.gameObject.SetActive(true);
                _textOver.GetComponent<RectTransform>().Rotate(0, 0, -1.2f);
                yield return new WaitForSeconds(1.5f);
                Debug.Log("_");
                _isGameOver = true;
                break;
        }
    }

    public void GameTitle() {
        Scene currentScene = SceneManager.GetActiveScene();//現在のシーンを読み込む
        _sceneIndex = currentScene.buildIndex;//現在のシーンのビルドインデックスを取得
        SceneManager.LoadScene(_sceneIndex - 1);//取得した値のシーンを読み込む
    }

    public void GameEnd() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                    Application.Quit();//ゲームプレイ終了
#endif
    }

}
