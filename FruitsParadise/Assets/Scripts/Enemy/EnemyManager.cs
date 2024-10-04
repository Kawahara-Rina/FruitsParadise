/*
    EnemyManager.cs

    �G�̓����Ȃǂ𐧌䂷��N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region �v���C�x�[�g�ϐ�

    private Vector3 screenLeftBottom;    // ��ʍ����̍��W�擾�p
    private EnemyGenerator eg;           // EnemyGenerator�擾�p

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
        eg = GameObject.Find("GameManager").GetComponent<EnemyGenerator>();
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // ��ʂ̈�ԉ����y���W���������Ȃ����I�u�W�F�N�g���i�[
        if (transform.position.y < screenLeftBottom.y - 1f)
        {
            eg.CollectEnemy(gameObject);
        }
    }
}
