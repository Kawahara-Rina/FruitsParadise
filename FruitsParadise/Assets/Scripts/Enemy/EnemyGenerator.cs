/*
    EnemyGenerator.cs

    �G�𐶐�����N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    #region �p�u���b�N�֐�

    #region GenerateEnemy - �G�l�~�[��������
    public GameObject GenerateEnemy()
    {
        // Queue�̒��g���m�F
        if (queueEnemy.Count <= 0)
        {
            // Queue����Ȃ̂�null��Ԃ�
            return null;
        }
        else
        {
            // Queue����1���o��
            var enemy = queueEnemy.Dequeue();
            enemy.SetActive(true);

            return enemy;
        }
    }
    #endregion

    #region CollectEnemy - �G�l�~�[�i�[����
    public void CollectEnemy(GameObject enemy)
    {
        // ��A�N�e�B�u��Ԃ�
        enemy.SetActive(false);

        // �f�t�H���g�|�W�V�����֖߂�
        enemy.transform.position = defPos;

        // Queue�Ɋi�[
        queueEnemy.Enqueue(enemy);
    }
    #endregion

    #endregion


    #region �v���C�x�[�g�ϐ�

    [SerializeField] GameObject prefabEnemy;     // �G�l�~�[�̃v���t�@�u
    [SerializeField] int maxCount;               // ���������

    private Queue<GameObject> queueEnemy;                // ���������G�l�~�[�̃I�u�W�F�N�g���i�[����L���[
    private Vector3 defPos = new Vector3(20f, 10f, 0f);  // �f�t�H���g�|�W�V����

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    // ����������
    private void Awake()
    {
        // �L���[�𐶐�
        queueEnemy = new Queue<GameObject>();

        // �G�l�~�[�𐶐�
        for (var i = 0; i < maxCount; i++)
        {
            // ����
            var enemy = Instantiate(prefabEnemy, defPos, Quaternion.identity);
            enemy.SetActive(false);

            // Queue�ɒǉ�
            queueEnemy.Enqueue(enemy);
        }
    }
    #endregion

    #endregion

}
