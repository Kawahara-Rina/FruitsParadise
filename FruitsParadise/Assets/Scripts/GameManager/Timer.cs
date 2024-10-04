/*
    Timer.cs

    �������Ԃ̊Ǘ����s���N���X�B
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    #region �p�u���b�N�ϐ�

    public float timer;            // �������ԗp�̃^�C�}�[
    public bool isGameStart;       // �Q�[�����n�܂������ǂ����̃t���O

    #endregion

    #region �p�u���b�N�֐�

    #region pauseButtonOnClick - �|�[�Y�{�^�������ŃQ�[�����ꎞ��~
    public void pauseButtonOnClick(){


    }
    #endregion

    #endregion


    #region �v���C�x�[�g�ϐ�

    [SerializeField] Text TimerText;         // �^�C�}�[�̃e�L�X�g�w��p
    [SerializeField] Text StartTimerText;    // �Q�[���J�n���̃J�E���g�_�E���̃e�L�X�g�w��p
    [SerializeField] GameObject timeupPanel; // �^�C���A�b�v���ɕ\������p�l���w��p

    [SerializeField] AudioClip countSE;      // �J�E���g���̌��ʉ�
    [SerializeField] AudioClip startSE;      // �Q�[���X�^�[�g���̌��ʉ�
    [SerializeField] AudioClip endSE;        // �Q�[���I�����̌��ʉ�

    [SerializeField] float lastSpurtTime;    // ���X�g�X�p�[�g�̕b��
    [SerializeField] float lsEnemy;          // ���X�g�X�p�[�g���̃G�l�~�[�̐����X�p��
    [SerializeField] float lsMinFruits;      // ���X�g�X�p�[�g���̃t���[�c�̐����X�p���A�����_���̍ŏ��l
    [SerializeField] float lsMaxFruits;      // ���X�g�X�p�[�g���̃G�l�~�[�̐����X�p���A�����_���̍ő�l


    private int seOnceCount = 0;       // ���ʉ�����x�����炷���߂Ɏg�p����ϐ�
    private int lastSpurtCount ;       // ���X�g�X�p�[�g�Ŏg���J�E���g

    private float countSETimer = 0;    // countSE��1�b��1��炷���߂Ɏg�p����ϐ�
    private float startTimer;          // �Q�[���J�n���̃J�E���g�_�E���̃^�C�}�[
   
    private GameManager gm;            // GameManager�擾
    private Animator animator;         // Animator�擾
    private AudioSource audioSource;   // AudioSource�擾

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ���������� 
    private void Awake()
    {
        // �Q�[���X�^�[�g���Ă��Ȃ����
        isGameStart = false;

        // ���X�g�X�p�[�g�ɓ��������̃J�E���g
        lastSpurtCount = 0;

        // �Q�[���J�n���̃^�C�}�[�����l
        startTimer = 3;

        // �^�C���A�b�v�p�l���͔�\��
        timeupPanel.SetActive(false);

        // �e�R���|�[�l���g�擾(GameManager,Animator,AudioSource)
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region TimeLimit - �Q�[�����̐������Ԃ̏���
    private void TimeLimit()
    {
        // �^�C�}�[��\������(�����_�ȉ��؂�̂ĕ\��)
        // Mathf.Ceil(timer) �� timer�̏����_��؂�̂Ă�
        TimerText.text = string.Format("{0}", Mathf.Ceil(timer));

        // �������Ԃ�1�b���J�E���g�_�E��
        timer -= Time.deltaTime;

        // �������Ԃ��߂Â��Ă�����^�C�}�[��_��
        if (timer <=10)
        {
            // �A�j���[�V������Blink�g���K�[���Z�b�g
            animator.SetTrigger("Blink");
        }

        // �^�C�}�[���}�C�i�X�̒l(0)�ɂȂ����ꍇ
        if (timer < 0)
        {
            // �Q�[���I���̃t���O�𗧂Ă�
            gm.isGameEnd = true;
            
            // �Q�[���I�����̌��ʉ���炷
            OnceSound(endSE);
            
            // �^�C���A�b�v�p�l����\������
            timeupPanel.SetActive(true);

            // �^�C�}�[��0�ƕ\��
            TimerText.text = string.Format("0");

            // �A�j���[�V������Idle�g���K�[�𔭉�
            animator.SetTrigger("Idle");
            
            // �Q�[���I�����琔�b�o�߂Ń��U���g��ʂ֑J��
            if (timer < -3)
            {
                SceneManager.LoadScene("ResultScene");
            }
        }
    }
    #endregion

    #region LastSpurt - ���X�g�X�p�[�g���Ɋe�v���t�@�u�̐����X�p����Z���ݒ�
    private void LastSpurt()
    {
        // ��x���������_���̒l����
        if (lastSpurtCount == 0)
        {
            // �N���̏o���X�p����Z���ݒ�
            gm.eSpan = lsEnemy;

            // �t���[�c�̏o���X�p����Z���ݒ�
            gm.fruitsSpan[Define.APPLE] = Random.Range(lsMinFruits, lsMaxFruits);
            gm.fruitsSpan[Define.CHERRY] = Random.Range(lsMinFruits, lsMaxFruits);
            gm.fruitsSpan[Define.PEACH] = Random.Range(lsMinFruits, lsMaxFruits);
            gm.fruitsSpan[Define.GRAPE] = Random.Range(lsMinFruits, lsMaxFruits);

            lastSpurtCount++;
        }
    }
    #endregion

    #region StartWaitTime - �Q�[�����J�n����܂Ő��b�҂���
    public void StartWaitTime()
    {
        //isGameStart = false;

        //startTimer = 3;
        // �X�^�[�g���̃e�L�X�g���\���ɂ���
        StartTimerText.enabled = true;

        // 1�b�Ɉ��J�E���g��SE��炷
        CountSound();

        // �^�C�}�[��\��
        TimerText.text = string.Format("{0}", Mathf.Ceil(timer));

        // �J�E���g�_�E���^�C�}�[��\������(�����_�ȉ���؂�̂ĕ\��)
        // Mathf.Ceil(timer) �� timer�̏����_��؂�̂Ă�
        StartTimerText.text = string.Format("{0}", Mathf.Ceil(startTimer));

        // �X�^�[�g���̃^�C�}�[��1�b���J�E���g�_�E��
        startTimer -= Time.deltaTime;

        // �^�C�}�[��0�ɂȂ�����
        if (startTimer <= 0)
        {
            // �Q�[���X�^�[�g���̌��ʉ�����x�����炷
            OnceSound(startSE);

            // �e�L�X�g�ɃQ�[���J�n�̕��������
            StartTimerText.text = string.Format("START !!");

            // �Q�[���X�^�[�g���Ă��琔�b�o�����Ȃ�Q�[�����J�n����
            if (startTimer < -1.3f)
            {
                // ���ʉ�����x�����炷�J�E���g��0�ŏ�����
                seOnceCount = 0;

                startTimer = 3;
                // �X�^�[�g���̃e�L�X�g���\���ɂ���
                StartTimerText.enabled = false;
                //pauseButton.SetActive(true);

                // �Q�[���X�^�[�g�̃t���O�𗧂Ă�
                isGameStart = true;
            }
        }
    }
    #endregion

    #region CountSound - countSE��1�b�Ɉ��炷����
    private void CountSound()
    {
        // 3,2,1�̃J�E���g����炷
        // 1�b�Ɉ��炷
        if (startTimer > 0)
        {
            countSETimer -= Time.deltaTime;
        }

        if (countSETimer <= 0)
        {
            audioSource.PlayOneShot(countSE);
            countSETimer = 1;
        }
    }
    #endregion

    #region OnceSound - �w�肵�����ʉ�����x�����炷����
    // ���ʉ�����x�����炷����
    private void OnceSound(AudioClip se)
    {
        // ��x��������炷
        if (seOnceCount == 0)
        {
            // �w�肵�����ʉ�����x�����炷
            audioSource.PlayOneShot(se);
            seOnceCount++;
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // �Q�[�����̏���
        if (isGameStart)
        {
            // �������Ԃ̊֐������s
            TimeLimit();

            // ���X�g�X�p�[�g�ɓ������ꍇ
            if (timer <= lastSpurtTime)
            {
                // ���X�g�X�p�[�g���̏��������s
                LastSpurt();
            }
        }
        // �Q�[���J�n�O�̏���
        else
        {
            // �Q�[���J�n�܂ł̃J�E���g�_�E���̎��s
            StartWaitTime();
        }
    }
}
