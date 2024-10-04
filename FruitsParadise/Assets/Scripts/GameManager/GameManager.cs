/*
    GameManager.cs 

    �Q�[�����̃��C���̏������s���N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region �p�u���b�N�ϐ�

    public float[] lines = new float[5];  // �v���C���[�̃��C�����W
    public float[] fruitsSpan;            // �e�t���[�c�̐����X�p��
    public float eSpan;                   // �G�l�~�[�̐����X�p��

    public bool isGameEnd;                // �Q�[�����I���������ǂ����̃t���O

    #endregion


    #region �v���C�x�[�g�ϐ�

    const int minLine = 0;
    const int maxLine = 5;

    [SerializeField] float spanMin;        // �e�t���[�c�����X�p���̍ŏ��l
    [SerializeField] float spanMax;        // �e�t���[�c�����X�p���̍ő�l

    [SerializeField] AudioClip fallSE;     // �I�u�W�F�N�g�������̌��ʉ�

    private float[] delta = new float[5];  // �e�t���[�c�E�G�l�~�[��

    private List<int> rNumbers = new List<int>();  // �����_���Ŏ擾�����l���i�[���郊�X�g

    private Vector3 screenRightTop;     // ��ʉE����W�̎擾�p

    private AudioSource audioSource;    // �I�[�f�B�I�\�[�X�擾�p

    private FruitsGenerator fg;         // FruitsGenerator�擾�p
    EnemyGenerator eg;                  // EnemyGenerator�擾�p
    private Timer tm;                   // Timer�擾�p

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        // �Q�[���͏I����Ă��Ȃ����
        isGameEnd = false;

        // �e�t���[�c�����X�p���������_���Ɍ���
        fruitsSpan[Define.APPLE] = Random.Range(spanMin, spanMax);
        fruitsSpan[Define.CHERRY] = Random.Range(spanMin, spanMax);
        fruitsSpan[Define.PEACH] = Random.Range(spanMin, spanMax);
        fruitsSpan[Define.GRAPE] = Random.Range(spanMin, spanMax);

        // �e�I�u�W�F�N�g��
        for(int i=0; i<delta.Length; i++)
        {
            delta[i] = 0;
        }

        // ���E���W�̎擾
        screenRightTop = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0));

        // �I�[�f�B�I�\�[�X�擾
        audioSource = GetComponent<AudioSource>();

        // �e�R���|�[�l���g�擾
        fg = GetComponent<FruitsGenerator>();
        eg = GetComponent<EnemyGenerator>();
        tm = GameObject.Find("timerImage").GetComponent<Timer>();
    }
    #endregion

    #region GenerateObject - �e�I�u�W�F�N�g��������
    /// <summary>
    /// 
    /// </summary>
    /// <param name="generateFunc">�����������I�u�W�F�N�g�̐����֐��Ăяo���Ɏg�p</param>
    private void GenerateObject(GameObject generateFunc)
    {
        // �������̌��ʉ���炷
        audioSource.PlayOneShot(fallSE);

        // �e�I�u�W�F�N�g�����̊֐����Ăяo���A�I�u�W�F�N�g�𐶐�
        var obj = generateFunc;

        if (obj != null)
        {
            // �I�u�W�F�N�g�����ʒu�������_���Ɍ���
            var line = Random.Range(minLine, maxLine);
            var x = lines[line];
            var y = Random.Range(screenRightTop.y - 0.5f, screenRightTop.y - 2);

            obj.transform.position = new Vector3(x, y, 0);
        }
    }
    #endregion

    #region GenerateJudge - �����̏����𖞂������I�u�W�F�N�g�𐶐�
    private void GenerateJudge()
    {
        // ��񂲐�������
        if (delta[Define.APPLE] > fruitsSpan[Define.APPLE])
        {
            delta[Define.APPLE] = 0;
            GenerateObject(fg.GenerateApple());
        }

        // �������ڐ�������
        if (delta[Define.CHERRY] > fruitsSpan[Define.CHERRY])
        {
            delta[Define.CHERRY] = 0;
            GenerateObject(fg.GenerateCherry());
        }

        // ������������
        if (delta[Define.PEACH] > fruitsSpan[Define.PEACH])
        {
            delta[Define.PEACH] = 0;
            GenerateObject(fg.GeneratePeach());
        }

        // �Ԃǂ���������
        if (delta[Define.GRAPE] > fruitsSpan[Define.GRAPE])
        {
            delta[Define.GRAPE] = 0;
            GenerateObject(fg.GenerateGrape());
        }

        // �G�l�~�[��������
        if (delta[Define.ENEMY] > eSpan)
        {
            delta[Define.ENEMY] = 0;
            GenerateObject(eg.GenerateEnemy());
        }
    }

    #endregion

    #region DeltaCountUp - �e�I�u�W�F�N�g�����܂ł̌o�ߎ��Ԃ��J�E���g�A�b�v
    private void DeltaCountUp()
    {
        // �e�I�u�W�F�N�g�̐����܂ł̌o�ߎ��Ԃ��J�E���g�A�b�v
        for (int i = 0; i < delta.Length; i++)
        {
            delta[i] += Time.deltaTime;
        }

    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // �������ԓ��ł����
        if (!isGameEnd && tm.isGameStart)
        {
            // �e�I�u�W�F�N�g�����܂ł̌o�ߎ��Ԃ��J�E���g�A�b�v
            DeltaCountUp();
        }

        // �e�I�u�W�F�N�g�̐����X�p���𒴂����ꍇ��
        // �e�I�u�W�F�N�g����ʒ��ɕ\��
        GenerateJudge();
    }
}
