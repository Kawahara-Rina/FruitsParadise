/*
    FruitsGenerator.cs

    �t���[�c�𐶐�����N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitsGenerator : MonoBehaviour
{
    #region �p�u���b�N�֐�

    #region GenerateApple - ��񂲐�������
    public GameObject GenerateApple()
    {
        // �L���[�̒��g���m�F
        if (queueApple.Count <= 0)
        {
            // Queue����Ȃ̂�null��Ԃ�
            return null;
        }
        else
        {
            // �L���[����1���o���Ԃ�
            var apple = queueApple.Dequeue();
            apple.SetActive(true);

            return apple;
        }
    }
    #endregion

    #region GenerateCherry - �������ڐ�������
    public GameObject GenerateCherry()
    {
        // Queue�̒��g���m�F
        if (queueCherry.Count <= 0)
        {
            // Queue����Ȃ̂�null��Ԃ�
            return null;
        }
        else
        {
            // Queue����1���o���Ԃ�
            var cherry = queueCherry.Dequeue();
            cherry.SetActive(true);

            return cherry;
        }
    }
    #endregion

    #region GeneratePeach - ������������
    public GameObject GeneratePeach()
    {
        // Queue�̒��g���m�F
        if (queuePeach.Count <= 0)
        {
            // Queue����Ȃ̂�null��Ԃ�
            return null;
        }
        else
        {
            // Queue����1���o���Ԃ�
            var peach = queuePeach.Dequeue();
            peach.SetActive(true);

            return peach;
        }
    }
    #endregion

    #region GenerateGrape - �Ԃǂ���������
    public GameObject GenerateGrape()
    {
        // Queue�̒��g���m�F
        if (queueGrape.Count <= 0)
        {
            // Queue����Ȃ̂�null��Ԃ�
            return null;
        }
        else
        {
            // Queue����1���o���Ԃ�
            var grape = queueGrape.Dequeue();
            grape.SetActive(true);

            return grape;
        }
    }
    #endregion

    #region CollectFruits - �e�t���[�c�i�[����
    public void CollectFruits(GameObject fruits)
    {
        // ��A�N�e�B�u��Ԃ�
        fruits.SetActive(false);

        // �f�t�H���g�|�W�V�����֖߂�
        fruits.transform.position = defPos;

        // �L���[�Ɋi�[����t���[�c���^�O�Ŕ��f
        switch (fruits.tag)
        {
            case Define.TAG_APPLE:
                queueApple.Enqueue(fruits);
                break;

            case Define.TAG_CHERRY:
                queueCherry.Enqueue(fruits);
                break;

            case Define.TAG_PEACH:
                queuePeach.Enqueue(fruits);
                break;

            case Define.TAG_GRAPE:
                queueGrape.Enqueue(fruits);
                break;
        }
    }
    #endregion

    #endregion


    #region �v���C�x�[�g�ϐ�

    // �e�t���[�c�̃v���t�@�u�i�[�p
    [SerializeField] GameObject prefabApple;    // ���
    [SerializeField] GameObject prefabCherry;   // ��������
    [SerializeField] GameObject prefabPeach;    // ����
    [SerializeField] GameObject prefabGrape;    // �Ԃǂ�

    [SerializeField] int maxCount;              // �v���t�@�u���������

    // �������ꂽ�e�t���[�c���i�[����L���[
    private Queue<GameObject> queueApple;
    private Queue<GameObject> queueCherry;
    private Queue<GameObject> queuePeach;
    private Queue<GameObject> queueGrape;

    private Vector3 defPos = new Vector3(15f, 10f, 0f); // �e�t���[�c�̃f�t�H���g�|�W�V����

    #endregion

    #region �v���C�x�[�g�֐�

    #region Awake - ����������
    private void Awake()
    {
        // �e�t���[�c���i�[����L���[�̐���
        queueApple = new Queue<GameObject>();
        queueCherry = new Queue<GameObject>();
        queuePeach = new Queue<GameObject>();
        queueGrape = new Queue<GameObject>();

        // �e�t���[�c�𐶐�
        for (var i = 0; i < maxCount; i++)
        {
            // ����
            var apple = Instantiate(prefabApple, defPos, Quaternion.identity);
            var cherry = Instantiate(prefabCherry, defPos, Quaternion.identity);
            var peach = Instantiate(prefabPeach, defPos, Quaternion.identity);
            var grape = Instantiate(prefabGrape, defPos, Quaternion.identity);

            apple.SetActive(false);
            cherry.SetActive(false);
            peach.SetActive(false);
            grape.SetActive(false);

            // Queue�ɒǉ�
            queueApple.Enqueue(apple);
            queueCherry.Enqueue(cherry);
            queuePeach.Enqueue(peach);
            queueGrape.Enqueue(grape);
        }
    }
    #endregion

    #endregion
    
}
