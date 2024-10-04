/*
    PlayerManager.cs

    �v���C���[�̓������Ǘ�����N���X�B
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    #region �p�u���b�N�ϐ�

    public static int fruitsScore;  // �t���[�c�l������
    public static int score;        // �X�R�A
    public static int highScore;    // �n�C�X�R�A

    // �e�t���[�c�̊l����
    public static int getApple;     // ���
    public static int getCherry;    // ��������
    public static int getGrape;     // �Ԃǂ�
    public static int getPeach;     // ����

    public static bool isHighScoreUpdate;  // �n�C�X�R�A���X�V�������ǂ����̃t���O
    public bool isFreeze;                  // �v���C���[���t���[�Y���Ă��邩�ǂ����̃t���O

    #endregion


    #region �v���C�x�[�g�ϐ�

    // �ړ�����
    private enum OperationType
    {
        RIGHT,
        LEFT,
        NONE
    }

    [SerializeField] Text scoreText;       // �X�R�A��\������e�L�X�g
    [SerializeField] Text getText;         // �t���[�c�Q�b�g���ɕ\������e�L�X�g
    [SerializeField] Image getImage;       // �t���[�c�Q�b�g���ɕ\������摜

    [SerializeField] AudioClip getSE;      // �t���[�c�Q�b�g���̌��ʉ�
    [SerializeField] AudioClip enemyGetSE; // �G�l�~�[�ɐG�ꂽ�ꍇ�̌��ʉ�

    [SerializeField] float speed;          // �ړ��X�s�[�h
    [SerializeField] float[] getTextPos = new float[5];  // getText�̍��W

    private int line;     // ���݂̃��C��
    private int nextLine; // �ړ���̃��C��

    private float textDeleteTimer; // �e�L�X�g����莞�Ԃŏ������߂̃^�C�}�[

    private bool isMove;  // �ړ����邩�ǂ����̃t���O
    private bool isGet;   // �������Q�b�g�������ǂ����̃t���O


    private GameManager gm;       // GameManager�擾�p
    private Timer tm;             // Timer�擾�p
    private FruitsGenerator fg;   // FruitsGenerator�擾�p
    private EnemyGenerator eg;    // EnemyGenerator�擾�p
    private Frame fr;             // Frame�擾�p

    private AudioSource audioSource;     // �I�[�f�B�I�\�[�X�擾�p
    private Animator pAnimator;          // �v���C���[�̃A�j���[�^�[�擾�p    
    private Animator getTextAnimator;    // getText�̃A�j���[�^�[�擾�p
    private Animator getImageAnimator;   // getImage�̃A�j���[�^�[�擾�p

    #endregion

    #region �v���C�x�[�g�֐�

    #region OnTriggerEnter2D - �N�����G�ꂽ�ꍇ�̏���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �G�ꂽ���̃^�O���擾
        var tag = collision.gameObject.tag;

        // �G�ꂽ���̂��N���ł����
        if (tag == "Spider")
        {
            // �v���C���[�̃t���[�Y�t���O�𗧂Ă�A�t���[�Y���̃A�j���[�V����
            isFreeze = true;
            pAnimator.SetTrigger("Freeze");

            // ���ʉ���炷
            audioSource.PlayOneShot(enemyGetSE);

            // �N�����ʒu�ɖ߂�
            eg.CollectEnemy(collision.gameObject);
        }
    }
    #endregion

    #region OnCollisionEnter2D - �t���[�c���Փ˂����ꍇ�̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �G�ꂽ�I�u�W�F�N�g���L���[�Ɋi�[�A��ʒu�ɖ߂�
        fg.CollectFruits(collision.gameObject);

        // �e�t���[�c���Q�b�g�����t���O�𗧂Ă�
        isGet = true;

        // �Q�b�g���̌��ʉ���炷
        audioSource.PlayOneShot(getSE);

        // �e�L�X�g�̃A�j���[�V�����A�g���K�[����
        getTextAnimator.SetTrigger("Get");
        // �C���[�W�̃A�j���[�V�����A�g���K�[����
        getImageAnimator.SetTrigger("Get");

        // tag : �Փ˂����R���W�����ɕt����ꂽ�e�t���[�c�̃^�O���擾
        var fruitsTag = collision.gameObject.tag;

        // �Փ˂����t���[�c�̃^�O�����āA�e�X�R�A�����Z����֐������s
        ScoreAddition(fruitsTag, collision);
    }
    #endregion

    #region Awake - �������֐�
    private void Awake()
    {
        // �e�X�R�A�̏�����
        fruitsScore = 0;
        score = 0;
        highScore = TitleManager.tHighScore;
        getApple = 0;
        getCherry = 0;
        getPeach = 0;
        getGrape = 0;

        // �n�C�X�R�A�X�V���Ă��Ȃ����
        isHighScoreUpdate = false;
        // �t���[�Y���Ă��Ȃ����
        isFreeze = false;
        // �ړ����Ă��Ȃ����
        isMove = false;

        // ���݃v���C���[�̂��郉�C�� �����ʒu ����
        line = 2;
        // �ړ���̃��C��
        nextLine = 2;

        // �^�C�}�[�̏����b��
        textDeleteTimer = 0;

        // �e�X�N���v�g�擾
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        fg = GameObject.Find("GameManager").GetComponent<FruitsGenerator>();
        eg = GameObject.Find("GameManager").GetComponent<EnemyGenerator>();
        fr = GameObject.Find("frameImage").GetComponent<Frame>();
        tm = GameObject.Find("timerImage").GetComponent<Timer>();

        // �I�[�f�B�I�\�[�X�擾
        audioSource = GetComponent<AudioSource>();

        // �v���C���[�̃A�j���[�^�[�擾
        pAnimator = GetComponent<Animator>();
        // getText�̃A�j���[�^�[�擾
        getTextAnimator = GameObject.Find("getText").GetComponent<Animator>();
        // getImage�̃A�j���[�^�[�擾
        getImageAnimator = GameObject.Find("getImage").GetComponent<Animator>();
    }
    #endregion

    #region ScoreAddition - �Փ˂������̃^�O�����āA�e�t���[�c�̃X�R�A�����Z����
    private void ScoreAddition(string fruitsTag, Collision2D collision)
    {

        // �t���[�c�̑��l������1���₷
        fruitsScore++;

        switch (fruitsTag)
        {
            // ��񂲂̏ꍇ
            case Define.TAG_APPLE:

                // ��񂲂̊l������1���₷
                getApple++;

                // ���ݑI�΂�Ă���t���[�c����񂲂̏ꍇ�͊l�����ɃX�R�A�𑝂₷
                if (fr.sFruits == Define.APPLE)
                {
                    score += 30;
                }
                // ��񂲈ȊO�̏ꍇ�͒ʏ�X�R�A
                else
                {
                    score += 10;
                }

                break;

            // �������ڂ̏ꍇ
            case Define.TAG_CHERRY:

                // �������ڂ̊l������1���₷
                getCherry++;

                // ���ݑI�΂�Ă���t���[�c���������ڂ̏ꍇ�͊l�����ɃX�R�A�𑝂₷
                if (fr.sFruits == Define.CHERRY)
                {
                    score += 30;
                }
                // �������ڈȊO�̏ꍇ�͒ʏ�X�R�A
                else
                {
                    score += 10;
                }

                break;

            // �Ԃǂ��̏ꍇ
            case "Grape":

                // �Ԃǂ��̊l������1���₷
                getGrape++;

                // ���ݑI�΂�Ă���t���[�c���Ԃǂ��̏ꍇ�͊l�����ɃX�R�A�𑝂₷
                if (fr.sFruits == Define.GRAPE)
                {
                    score += 30;
                }
                // �Ԃǂ��ȊO�̏ꍇ�͒ʏ�X�R�A
                else
                {
                    score += 10;
                }

                break;

            // �����̏ꍇ
            case "Peach":

                // ��񂲂̊l������1���₷
                getPeach++;

                // ���ݑI�΂�Ă���t���[�c�������̏ꍇ�͊l�����ɃX�R�A�𑝂₷
                if (fr.sFruits == Define.PEACH)
                {
                    score += 30;
                }
                // �����ȊO�̏ꍇ�͒ʏ�X�R�A
                else
                {
                    score += 10;
                }

                break;
        }
    }
    #endregion

    #region ShowGetObject - �t���[�c�Q�b�g���Ƀe�L�X�g��C���[�W��\������
    private void ShowGetObjects()
    {
        // getText�̕\��
        getText.enabled = true;
        // getImage�̕\��
        getImage.enabled = true;

        // getText�̈ʒu���A�v���C���[�̍�������W��
        getText.transform.localPosition = new Vector3(getTextPos[nextLine], -250, 0);
        // getImage�̈ʒu���A�v���C���[�̍�������W��
        getImage.transform.localPosition = new Vector3(getTextPos[nextLine] - 105, -181, 0);

        // text,Image�𐔕b�\�����邽�߂ɁA�J�E���g�A�b�v
        textDeleteTimer += Time.deltaTime;

        // �^�C�}�[�ň��b�o��
        // �܂��͂��̏ꂩ�瓮����getText,Image���\��
        if (textDeleteTimer >= 1.5f || isMove)
        {
            // text���\���ɂ���
            getText.enabled = false;
            // image���\���ɂ���
            getImage.enabled = false;

            isGet = false;

            // ��\���ɂ���܂ł̃^�C�}�[�̏�����
            textDeleteTimer = 0;
        }
    }
    #endregion

    #region ShowFruitsScore - �t���[�c�̑��l������\��
    private void ShowFruitsScore()
    {
        // �t���[�c�̑��l������\��
        scoreText.text = string.Format("{0}", fruitsScore);
    }
    #endregion

    #region JudgeHighScore - �n�C�X�R�A���X�V���������f
    private void JudgeHighScore()
    {
        // �X�R�A���n�C�X�R�A�𒴂�����n�C�X�R�A�ɂ���
        if (TitleManager.tHighScore < score)
        {
            // �n�C�X�R�A���X�V���ꂽ�t���O�𗧂Ă�
            isHighScoreUpdate = true;
            highScore = score;
        }
    }
    #endregion

    #region GetDirection - �L�[���͂������𔻒f���A�ړ�������Ԃ��֐�
    private OperationType GetDirection()
    {
        // ret ���͕�����Ԃ��l
        OperationType ret = OperationType.NONE;

        // �E���L�[�����͂��ꂽ�ꍇ
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // ret�ɉE�����ړ����i�[
            ret = OperationType.RIGHT;
        }
        // �����L�[�����͂��ꂽ�ꍇ
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // ret�ɍ������ړ����i�[
            ret = OperationType.LEFT;
        }
        else
        {
            // ��L�ӊO�͈ړ����Ȃ�
            ret = OperationType.NONE;
        }

        // �ړ�������Ԃ�
        return ret;
    }
    #endregion

    #region DirectionCheck - �ړ������`�F�b�N����
    private void DirectionCheck()
    {
        // direction : GetDirection�֐��ŕԂ��ꂽ�l(�eOperationType)���i�[
        var direction = GetDirection();

        // direction�̒l�ɂ�菈����ς���
        switch (direction)
        {
            // OperationType.RIGHT : �E�����ɃX���C�v���ꂽ�ꍇ
            // �v���C���[�̍����郉�C�����E�[�ȊO�̏ꍇ�A���̃��C����1���₵�A�E�Ɉړ�
            case OperationType.RIGHT:
                if (line < gm.lines.Length - 1)
                {
                    nextLine = line + 1;
                }
                isMove = true;
                break;

            // OperationType.LEFT : �������ɃX���C�v���ꂽ�ꍇ
            // �v���C���[�̍����郉�C�������[�ȊO�̏ꍇ�A���̃��C����1���炵�A���Ɉړ�
            case OperationType.LEFT:
                if (line > 0)
                {
                    nextLine = line - 1;
                }
                isMove = true;
                break;

            default:
                break;
        }
    }
    #endregion

    #region Move - �v���C���[�ړ�����
    private void Move()
    {
        // pos : �v���C���[�̌��ݍ��W���i�[
        var pos = transform.position;

        // �E�Ɉړ��̏ꍇ
        if (line < nextLine)
        {
            // x���v���X�����Ɉړ�
            transform.position += new Vector3(speed, 0, 0);

            // ���̃��C���܂ňړ�������~�߂�
            if (transform.position.x >= gm.lines[nextLine])
            {
                // ���݂̃��C�����ړ���̃��C����
                line = nextLine;
                isMove = false;

                // �ړ����ɁA�������W�̂��ꂪ������ꍇ�����邽��gm.lines�Ɋi�[���ꂽ���W�ɔz�u���Ȃ���
                transform.position = new Vector3(gm.lines[line], pos.y, pos.z);
            }
        }
        // ���Ɉړ��̏ꍇ
        else if (line > nextLine)
        {
            // x���}�C�i�X�����Ɉړ�
            transform.position += new Vector3(-speed, 0, 0);

            // ���̃��C���܂ňړ�������~�߂�
            if (transform.position.x <= gm.lines[nextLine])
            {
                // ���݂̃��C�����ړ���̃��C����
                line = nextLine;
                isMove = false;

                // �ړ����ɁA�������W�̂��ꂪ������ꍇ�����邽��gm.lines�Ɋi�[���ꂽ���W�ɔz�u���Ȃ���
                transform.position = new Vector3(gm.lines[line], pos.y, pos.z);
            }
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // �t���[�c�̑��l������\��
        ShowFruitsScore();

        // �Q�[�����A�t���[�Y�w��Ȃ��ꍇ�̓L�[���͂��󂯕t����
        if (tm.isGameStart && !isFreeze)
        {
            // �ړ������̃`�F�b�N
            DirectionCheck();
        }

        // �ړ��t���O���������Ă���ꍇ
        if (isMove)
        {
            // �v���C���[�ړ������̊֐������s
            Move();
        }

        // �e�t���[�c���Q�b�g�����ꍇ
        if (isGet)
        {
            // �Q�b�g���̃e�L�X�g��C���[�W��\������֐������s
            ShowGetObjects();
        }

        // �n�C�X�R�A���X�V�������𔻒f����֐������s
        JudgeHighScore();
    }
}
