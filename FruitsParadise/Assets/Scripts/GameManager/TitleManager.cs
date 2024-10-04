/*
    TitleManager.cs

    �^�C�g����ʂł̏������s���N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{
    #region �p�u���b�N�ϐ�

    public static int tHighScore;  // �^�C�g����ʂŕ\������n�C�X�R�A

    #endregion

    #region �p�u���b�N�֐�

    #region StartButtonOnClick - �X�^�[�g�{�^�������ŃQ�[���X�^�[�g�̃t���O�𗧂Ă�
    public void StartButtonOnClick()
    {
        // �X�^�[�g�̃t���O�𗧂Ă�
        isStart = true;
    }
    #endregion

    #region NextButtonOnClick - �V�ѕ������̎��փ{�^�������Ŏ��̉摜�ɐ؂�ւ�
    public void NextButtonOnClick()
    {
        // �V�ѕ��������Ō�̃y�[�W�łȂ���Ύ��֐i��
        if (imageNumber != ruleImages.Length - 1)
        {
            // �z��̓Y����������
            imageNumber++;
        }

        // ���̉摜��\��
        ruleImages[imageNumber].enabled = true;

        // �ЂƂO�̉摜���\��
        ruleImages[imageNumber - 1].enabled = false;
    }
    #endregion

    #region BackButtonOnClick - �V�ѕ������̖߂�{�^�������őO�̉摜�ɐ؂�ւ�
    public void BackButtonOnClick()
    {
        // �V�ѕ��������ŏ��̃y�[�W�łȂ���΂ЂƂO�֖߂�
        if (imageNumber != 0)
        {
            // �z��̓Y��������O��
            imageNumber--;
        }
        // ���̉摜��\��
        ruleImages[imageNumber].enabled = true;

        // �ЂƂO�̉摜���\����
        ruleImages[imageNumber + 1].enabled = false;
    }
    #endregion

    #region HowtoPlayButtonOnClick - �V�ѕ��{�^�������ŗV�ѕ�������\��
    public void HowtoPlayButtonOnClick()
    {
        // �����т���������\��
        ruleImages[imageNumber].enabled = true;
    }
    #endregion

    #region ExitButtonOnClick - �V�ѕ����������{�^�������ŗV�ѕ��������\��
    public void ExitButtonOnClick()
    {
        // �V�ѕ��������\��
        ruleImages[imageNumber].enabled = false;
    }
    #endregion

    #endregion


    #region �v���C�x�[�g�ϐ�

    [SerializeField] Text highScoreText;                // �n�C�X�R�A�̃e�L�X�g
    [SerializeField] Image[] ruleImages = new Image[4]; // ���[�������̉摜�i�[�p�z��
    [SerializeField] GameObject rabbitImage;            // �������̉摜�i�[�p

    [SerializeField] float wait;       // ��ʑJ�ڎ����b�҂ꍇ�Ɏg�p

    [SerializeField] AudioClip tapSE;  // �^�b�v���̌��ʉ�

    private int imageNumber;           // �z��(ruleImages)�̓Y�����ύX�A�w��Ɏg�p
    private bool isStart;              // �X�^�[�g�{�^���������ꂽ���ǂ����̃t���O

    private AudioSource audioSource;   // AudioSource�擾�p
    private Animator rAnimator;        //�@�������̃A�j���[�^�[�擾�p

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - �������֐�
    private void Awake()
    {
        // tHighScore�� ���U���g��ʂōŌ�ɋL�^���ꂽ�n�C�X�R�A���i�[
        tHighScore = ResultManager.rHighScore;

        // �n�C�X�R�A��\��
        highScoreText.text = string.Format("�n�C�X�R�A�F{0}", tHighScore);

        // �z��̓Y�����p�ϐ��̏�����
        imageNumber = 0;

        // �z��(ruleImage)�̊e�v�f���A�N�e�B�u�ŏ�����
        for (int i = 0; i < ruleImages.Length; i++)
        {
            ruleImages[i].enabled = false;
        }

        // �Q�[���̓X�^�[�g���Ă��Ȃ����
        isStart = false;

        // �I�[�f�B�I�\�[�X���擾
        audioSource = GetComponent<AudioSource>();
        // �������̃A�j���[�^�[���擾
        rAnimator = GameObject.Find("rabbitImage").GetComponent<Animator>();
    }
    #endregion

    #region ToGameScene - GameScene�֑J�ڂ���֐�
    private void ToGameScene()
    {
        //�Q�[���V�[���֑J��
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    #region RabbitAnimation - ��ʃ^�b�v���ɂ������̃A�j���[�V����������֐�
    private void RabbitAnimation()
    {
        // �^�b�v�����ʉ���炷
        audioSource.PlayOneShot(tapSE);

        // Ray�𔭎˂��āA��ʂ��^�b�v�������ǂ������ׂ�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        // Ray�ŉ����q�b�g���Ȃ������ꍇ�� ��ʂ��^�b�v���ꂽ
        // �q�b�g�����ꍇ�ł�UI�I�u�W�F�N�g�łȂ��ꍇ
        if (!hit2d || hit2d.transform.gameObject.tag != "UIObject")
        {
            // ���݂̂������̃A�j���[�V�����������Ă����ԂłȂ����
            if (!rAnimator.GetCurrentAnimatorStateInfo(0).IsName("move@titleRabbit"))
            {
                // Tap�g���K�[�𔭉΁A�����Ă���A�j���[�V�����֑J��
                rAnimator.SetTrigger("Tap");
            }
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // �X�^�[�g�{�^���������ꂽ�ꍇ
        if (isStart)
        {
            // 2�b��ɃQ�[���V�[���ɑJ�ڂ���֐������s
            Invoke("ToGameScene", wait);
        }

        // �N���b�N���ꂽ�ꍇ
        if (Input.GetMouseButtonDown(0))
        {
            // �������̃A�j���[�V����������֐������s
            RabbitAnimation();
        }

    }
}
