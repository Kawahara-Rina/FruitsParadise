/*
    Frame.cs

    �v���C���[�ׂ̗ɕ\������鐁���o���̊Ǘ����s���N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    #region �p�u���b�N�ϐ�

    public int sFruits;      // �����_���ɑI�΂ꂽ�t���[�c���i�[����ϐ�

    #endregion


    #region �v���C�x�[�g�ϐ�

    [SerializeField] int callSeconds;  // ���b���ƂɊ֐����Ăяo�����̕b�����i�[����ϐ�
    private Animator animator;         // �A�j���[�^�[�擾

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        // �A�j���[�^�[�̃R���|�[�l���g���擾
        animator = GetComponent<Animator>();

        // ShowFrame�֐���callSeconds�x�ɌĂяo��
        InvokeRepeating("ShowFrame", 1, callSeconds);
    }
    #endregion

    #region SelectFruits - �e�t���[�c���烉���_���Ɉ�I��
    private void SelectFruits()
    {
        // �e�t���[�c���烉���_���ɑI��
        sFruits = Random.Range(Define.APPLE, Define.GRAPE);
    }
    #endregion

    #region ShowFrame - �I�΂ꂽ�t���[�c�Ƃӂ�������\������
    private void ShowFrame()
    {
        // �e�t���[�c�����I��
        SelectFruits();

        // �I�΂ꂽ�t���[�c�͉����𔻒f
        // �A�j���[�V�����̃g���K�[�̃Z�b�g(�e�ӂ������ƁA�I�΂ꂽ�t���[�c���\�������A�j���[�V����)
        switch (sFruits)
        {
            // ��񂲂̏ꍇ
            case Define.APPLE:
                animator.SetTrigger("fApple");
                break;

            // �������ڂ̏ꍇ
            case Define.CHERRY:
                animator.SetTrigger("fCherry");
                break;

            // �����̏ꍇ
            case Define.PEACH:
                animator.SetTrigger("fPeach");
                break;

            // �Ԃǂ��̏ꍇ
            case Define.GRAPE:
                animator.SetTrigger("fGrape");
                break;
        }
    }
    #endregion

    #endregion

}