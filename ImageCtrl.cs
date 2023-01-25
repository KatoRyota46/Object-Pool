using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCtrl : MonoBehaviour {
    public Image _hpGauge;//HP�Q�[�W�i�F�t���j
    public Image _expGauge;//EXP�Q�[�W�i�F�t���j
    public Text _levelText;//���x���e�L�X�g
    public Text _textGame;//�Q�[���I�[�o�[�e�L�X�g�@
    public Text _textOver;//�Q�[���I�[�o�[�e�L�X�g�A
    public Text _textGameClear;//�Q�[���N���A�e�L�X�g
    public Text _textTime;
    private PlayerCtrl _player;//�v���C���[�X�N���v�g
    private EnemyCtrl _enemy;//�G�X�N���v�g
    private int _imageHp;//�Q�[�W���̌���HP
    private int _imageMaxHp;//�Q�[�W���̍ő�HP
    private int _imageExp;//�Q�[�W���̌���EXP
    private int _imagePreviewNeedExp;//�Q�[�W���̑O��EXP
    private int _imageNeedExp;//�Q�[�W���̎���EXP
    private float _gameClearBorder = 60f;
    [SerializeField]
    private float _gameClearTime = 0f;
    private bool _isGameOver = false;

    void Update() {
        //�v���C���[�擾
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;

        _gameClearTime += Time.deltaTime;
        _textTime.text = _gameClearTime.ToString("f1");

        //HP�̃Q�[�W�̕\�����X�V����
        _imageHp = _player.PlayerHp;
        _imageMaxHp = _player.PlayerMaxHp;
        _hpGauge.fillAmount = (float)_imageHp / _imageMaxHp;

        //EXP�̃Q�[�W�̕\�����X�V����
        _imageExp = _player.PlayerExp;
        _imagePreviewNeedExp = _player.PreviewNeedExp;
        _imageNeedExp = _player.NeedExp;
        _expGauge.fillAmount = (float)(_imageExp - _imagePreviewNeedExp) / (_imageNeedExp - _imagePreviewNeedExp);

        //���x���̕\�����X�V����
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
                UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
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
