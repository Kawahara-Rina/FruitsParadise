/*
    Miss.cs

    �t���[�c����蓦�����ۂ̏������s���N���X�B
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miss : MonoBehaviour
{
    #region �v���C�x�[�g�ϐ�

    private Animator animator;  //�A�j���[�^�[���擾

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        // �A�j���[�^�[�̃R���|�[�l���g���擾
        animator = GetComponent<Animator>();
    }

    #endregion

    #region OnTriggerEnter2D - �t���[�c���G�ꂽ�ꍇ�e�L�X�g��\��(�A�j���[�V����)����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �G�ꂽ�I�u�W�F�N�g�̃^�O���擾
        var tag = collision.gameObject.tag;

        // �G�ꂽ�����N���ȊO�ł����(�t���[�c�Ȃ��)
        if (tag != Define.TAG_ENEMY)
        {
            // �A�j���[�V�����̃g���K�[ Miss���Z�b�g
            // �t���[�c�����Ȃ������ꍇ�A��ʉ���MISS��\������A�j���[�V����
            animator.SetTrigger("Miss");
        }
    }
    #endregion

    #endregion

}
