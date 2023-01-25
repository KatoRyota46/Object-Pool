using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCtrl : MonoBehaviour {
    public Image _hpGauge;//HPゲージ（色付き）
    public Image _expGauge;//EXPゲージ（色付き）
    public Text _levelText;//レベルテキスト
    public Text _textGame;//ゲームオーバーテキスト①
    public Text _textOver;//ゲームオーバーテキスト②
    public Text _textGameClear;//ゲームクリアテキスト
    public Text _textTime;
    private PlayerCtrl _player;//プレイヤースクリプト
    private EnemyCtrl _enemy;//敵スクリプト
    private int _imageHp;//ゲージ側の現在HP
    private int _imageMaxHp;//ゲージ側の最大HP
    private int _imageExp;//ゲージ側の現在EXP
    private int _imagePreviewNeedExp;//ゲージ側の前のEXP
    private int _imageNeedExp;//ゲージ側の次のEXP
    private float _gameClearBorder = 60f;
    [SerializeField]
    private float _gameClearTime = 0f;
    private bool _isGameOver = false;

    void Update() {
        //プレイヤー取得
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;

        _gameClearTime += Time.deltaTime;
        _textTime.text = _gameClearTime.ToString("f1");

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

    private void GameState() {
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;
        if (_gameClearTime >= _gameClearBorder && _enemy._isGameClear && _player.Residue > 0) {
            _textGameClear.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if (_player.Residue == 0 && !_enemy._isGameClear) {
            StartCoroutine(GameOverStaging());
            if (_isGameOver) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
            }
        }
    }

    private IEnumerator GameOverStaging() {
        Time.timeScale = 0;
        _textGame.gameObject.SetActive(true);
        _textOver.gameObject.SetActive(true);
        _textOver.GetComponent<RectTransform>().Rotate(0, 0, -1.2f);
        Time.timeScale = 1;
        yield return new WaitForSeconds(1.5f);
        _isGameOver = true;
    }

}
