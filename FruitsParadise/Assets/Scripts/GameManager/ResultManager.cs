/*
    ResultManager.cs

    ���U���g�������s���N���X
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    #region �p�u���b�N�ϐ�

    // �e�X�R�A�̃e�L�X�g
    public Text scoreText;               // �l���X�R�A�̃e�L�X�g
    public Text scoreTotalText;          // �l���t���[�c���̃e�L�X�g
    public Text highscoreUpdateText;     // �n�C�X�R�A�X�V���̃e�L�X�g
    public Text scoreAppleText;          // ��񂲂̊l�����̃e�L�X�g
    public Text scoreCherryText;         // �������ڂ̊l�����̃e�L�X�g
    public Text scorePeachText;          // �����̊l�����̃e�L�X�g
    public Text scoreGrapeText;          // �Ԃǂ��̊l�����̃e�L�X�g

    public static int rHighScore = 300;  // ���U���g��ʂŕ\������n�C�X�R�A

    #endregion

    #region �p�u���b�N�֐�

    #region TitleButtonOnClick - �^�C�g���{�^�������Ń^�C�g����ʂɑJ��
    public void TitleButtonOnClick()
    {
        // �^�C�g����ʂɑJ��
        SceneManager.LoadScene("TitleScene");
    }
    #endregion

    #region RetryButtonOnClick - ���g���C�{�^�������ŃQ�[����ʂɑJ��
    public void RetryButtonOnClick()
    {
        // �Q�[����ʂɑJ��
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    #endregion


    #region �v���C�x�[�g�ϐ�

    // �ォ�痎�Ƃ��e�t���[�c�̃v���t�@�u�i�[�p
    [SerializeField] GameObject prefabApple;    // ���
    [SerializeField] GameObject prefabCherry;   // ��������
    [SerializeField] GameObject prefabPeach;    // ����
    [SerializeField] GameObject prefabGrape;    // �Ԃǂ�

    [SerializeField] int randomX;               // �����_���Ő������鐮���͈̔͊i�[�p
    [SerializeField] int startY;                // �����_���Ő�������y���W�̍ŏ��l
    [SerializeField] int endY;                  // �����_���Ő�������y���W�̍ő�l

    [SerializeField] float countSpeed;          // �X�R�A���J�E���g����X�s�[�h
    [SerializeField] float maxY;                // y���W�̏���l

    private float scoreCount;                   // �X�R�A�̃J�E���g�A�b�v�\���Ɏg�p
    
    private bool isCountSkip;                   // �X�R�A�̃J�E���g�A�b�v���X�L�b�v�������ǂ����̃t���O

    private Vector3 defPos;                     // �e�t���[�c�����̃f�t�H���g�|�W�V����

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        // rHighScore�ɑO��̃Q�[���Ŋl�������n�C�X�R�A���i�[
        rHighScore = PlayerManager.highScore;

        // �n�C�X�R�A�X�V�̃e�L�X�g���\��
        highscoreUpdateText.enabled = false;

        // �X�R�A��0����1�����₵�Ȃ���\�����邽�߁A0�ŏ�����
        scoreCount = 0;

        // �X�R�A�\�����X�L�b�v����Ă��Ȃ����
        isCountSkip = false;

        // �e�t���[�c�����̃f�t�H���g�|�W�V����
        defPos = new Vector3(0f, 8f, 0f);

        // ���ꂼ��̃t���[�c���l����������������
        FruitsFall(PlayerManager.getApple, prefabApple);
        FruitsFall(PlayerManager.getCherry, prefabCherry);
        FruitsFall(PlayerManager.getPeach, prefabPeach);
        FruitsFall(PlayerManager.getGrape, prefabGrape);
    }
    #endregion

    #region ShowHighScoreText - �n�C�X�R�A���X�V����Ă�����X�R�A�X�V�̃e�L�X�g��\��
    private void ShowHighScoreText()
    {
        // �n�C�X�R�A���X�V����Ă����ꍇ
        if (PlayerManager.isHighScoreUpdate)
        {
            // �X�R�A�X�V�̃e�L�X�g��\������
            highscoreUpdateText.enabled = true;
        }
    }

    #endregion

    #region ShowScore - �X�R�A�J�E���g�A�b�v�\���X�L�b�v���ɕ\������X�R�A
    private void ShowScore()
    {
        // ���ꂼ��̃X�R�A����x�ɕ\������
        scoreText.text = string.Format("{0}", PlayerManager.score);
        scoreTotalText.text = string.Format("{0}", PlayerManager.fruitsScore);
        scoreAppleText.text = string.Format("�~ {0}", PlayerManager.getApple);
        scoreCherryText.text = string.Format("�~ {0}", PlayerManager.getCherry);
        scorePeachText.text = string.Format("�~ {0}", PlayerManager.getPeach);
        scoreGrapeText.text = string.Format("�~ {0}", PlayerManager.getGrape);
    }
    #endregion

    #region SkipShowScore - �X�R�A�J�E���g�A�b�v�\�����X�L�b�v���ꂽ�ꍇ�ɁA��x�ɒl��\������
    // �X�R�A�̃J�E���g�A�b�v�\�����X�L�b�v���ꂽ�ꍇ�ɁA��C�ɒl��\������֐�
    private void SkipShowScore()
    {
        // �X�L�b�v����Ă��Ȃ��ꍇ
        if (isCountSkip == false)
        {
            // �^�b�v���ꂽ��
            if (Input.GetMouseButtonDown(0))
            {
                // �X�L�b�v�̃t���O�𗧂Ă�
                isCountSkip = true;
            }
        }

        // �X�L�b�v���ꂽ�ꍇ
        else
        {
            // �n�C�X�R�A���X�V����Ă�����X�V�̕�����\������
            ShowHighScoreText();

            // ���ꂼ��̃X�R�A����C�ɕ\������
            ShowScore();
        }
    }

    #endregion

    #region FruitsFall - �t���[�c���ォ��~���Ă��鉉�o
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fruits">��������t���[�c�̐�</param>
    /// <param name="prefab">��������v���t�@�u</param>
    void FruitsFall(int fruits, GameObject prefab)
    {
        // �l�������t���[�c�̐����J��Ԃ��A�v���t�@�u�𐶐�����
        for (var i = 0; i < fruits; i++)
        {
            // �v���t�@�u�𐶐�������W��x���W�͂܂΂��
            // randomX : �����_���͈̔͂��w�肵���ϐ�
            var x = Random.Range(-randomX, randomX);

            // �t���[�c�̊l�������v���t�@�u�𐶐�����
            Instantiate(prefab, defPos, Quaternion.identity);

            // ��������ʒu��x���W�̓����_���Ŏ擾�����l
            // ��������ʒu��y���W�͏����������Ă���悤�ɏォ��
            defPos.x = x;
            defPos.y += i;       

            // ��������ʒu��y���W���傫���Ȃ肷�����ꍇ
            if (defPos.y >= maxY)
            {
                // y���W�̈ʒu���߂ɒ���
                var y = Random.Range(startY, endY);
                defPos.y = y;
            }
        }
    }

    #endregion

    #region ScoreCount - �X�R�A(���U���g)���P�����₵�Ȃ���\������
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_score">�ǂ̃X�R�A�����w��</param>
    /// <param name="_text">�ǂ̃e�L�X�g�ɕ\�����邩���w��</param>
    /// <param name="fruit">�X�R�A�̓t���[�c���A�����X�R�A�����w��</param>
    void ScoreCount(int _score, Text _text, bool fruit)
    {
        // �X�L�b�v����Ă��Ȃ��ꍇ
        if (isCountSkip == false)
        {
            //�X�R�A���P�����₵�Ȃ���\������
            if (scoreCount < _score)
            {
                // �t���[�c�l�����\���̏ꍇ
                if (fruit)
                {
                    // �t���[�c �~�@�l�����@�@�ƕ\��
                    _text.text = string.Format("�~ {0}", (int)scoreCount);
                }
                // �X�R�A�\���̏ꍇ
                else
                {
                    // scoreCount�����������Z
                    scoreCount += countSpeed;

                    // �X�R�A��\��
                    _text.text = string.Format("{0}", (int)scoreCount);
                }
            }
            // �X�R�A�̉��Z�\�����I�����ꍇ
            else
            {
                if (fruit)
                {
                    // �l������\��
                    _text.text = string.Format("�~ {0}", _score);
                }
                else
                {
                    // �X�R�A��\��
                    _text.text = string.Format("{0}",_score);
                }
            }
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // �X�R�A�̃J�E���g�A�b�v�\�����X�L�b�v���ꂽ�ꍇ�̏���
        SkipShowScore();

        // �X�R�A�A�e�t���[�c�l�������J�E���g�A�b�v�\��
        ScoreCount(PlayerManager.score, scoreText, false);
        ScoreCount(PlayerManager.fruitsScore, scoreTotalText, false);
        ScoreCount(PlayerManager.getApple, scoreAppleText, true);
        ScoreCount(PlayerManager.getCherry, scoreCherryText, true);
        ScoreCount(PlayerManager.getPeach, scorePeachText, true);
        ScoreCount(PlayerManager.getGrape, scoreGrapeText, true);
    }
}
