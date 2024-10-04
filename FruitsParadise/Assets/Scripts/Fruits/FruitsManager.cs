/*
    FruitsManager.cs

    �e�t���[�c���Ǘ�����N���X�B 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitsManager : MonoBehaviour
{
    #region �v���C�x�[�g�ϐ�

    private Vector3 screenLeftBottom;   // ��ʍ����̍��W�i�[�p

    private FruitsGenerator fg;         // FruitsGenerator�擾�p

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        // ��ʂ̍����̍��W���擾
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // FruitsGenerator�擾
        fg = GameObject.Find("GameManager").GetComponent<FruitsGenerator>();
    }
    #endregion

    #endregion


    // Update is called once per frame
    void Update()
    {
        // ��ʂ̈�ԉ����y���W���������Ȃ����I�u�W�F�N�g���i�[
        if (transform.position.y < screenLeftBottom.y - 1f)
        {
            fg.CollectFruits(gameObject);
        }
    }
}
