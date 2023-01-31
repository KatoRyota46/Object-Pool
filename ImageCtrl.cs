using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ImageCtrl : MonoBehaviour {
    public Image _hpGauge;//HP�Q�[�W�i�F�t���j
    public Image _expGauge;//EXP�Q�[�W�i�F�t���j
    public Text _levelText;//���x���e�L�X�g
    public Text _textGame;//�Q�[���I�[�o�[�e�L�X�g�@
    public Text _textOver;//�Q�[���I�[�o�[�e�L�X�g�A
    public Text _textGameClear;//�Q�[���N���A�e�L�X�g
    private PlayerCtrl _player;//�v���C���[�X�N���v�g
    private EnemyCtrl _enemy;//�G�X�N���v�g
    private int _imageHp;//�Q�[�W���̌���HP
    private int _imageMaxHp;//�Q�[�W���̍ő�HP
    private int _imageExp;//�Q�[�W���̌���EXP
    private int _imagePreviewNeedExp;//�Q�[�W���̑O��EXP
    private int _imageNeedExp;//�Q�[�W���̎���EXP
    private bool _isGameClear = false;//ImageCtrl���̃Q�[���N���A�t���O
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
        //�v���C���[�擾
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;

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

    /// <summary>
    /// �Q�[�����U���g
    /// �Q�[���I�����̏󋵂ɂ���ăt���O�̃I���I�t�����܂�
    /// </summary>
    private void GameState() {
        _player = PlayerCtrl.PlayerInstance;
        _enemy = EnemyCtrl.EnemyInstance;
        /*Enemy.Ctrl���̃Q�[���N���A�t���O���I�����A�c�0�ȏ�ł����
         * �Q�[���N���A
         */
        if (_enemy.IsGameClear && _player.Residue >= 0) {
            StartCoroutine(GameStaging(_isStatsValue = true));
            if (_isGameClear) {
                GameTitle();
            }
        }
        /*Enemy.Ctrl���̃Q�[���N���A�t���O���I�t���A�c��Ȃ��Ȃ��
         * �Q�[���I�[�o�[
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
        Scene currentScene = SceneManager.GetActiveScene();//���݂̃V�[����ǂݍ���
        _sceneIndex = currentScene.buildIndex;//���݂̃V�[���̃r���h�C���f�b�N�X���擾
        SceneManager.LoadScene(_sceneIndex - 1);//�擾�����l�̃V�[����ǂݍ���
    }

    public void GameEnd() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
                    Application.Quit();//�Q�[���v���C�I��
#endif
    }

}
