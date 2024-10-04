/*
    FruitsGenerator.cs

    フルーツを生成するクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitsGenerator : MonoBehaviour
{
    #region パブリック関数

    #region GenerateApple - りんご生成処理
    public GameObject GenerateApple()
    {
        // キューの中身を確認
        if (queueApple.Count <= 0)
        {
            // Queueが空なのでnullを返す
            return null;
        }
        else
        {
            // キューから1個取り出し返す
            var apple = queueApple.Dequeue();
            apple.SetActive(true);

            return apple;
        }
    }
    #endregion

    #region GenerateCherry - さくらんぼ生成処理
    public GameObject GenerateCherry()
    {
        // Queueの中身を確認
        if (queueCherry.Count <= 0)
        {
            // Queueが空なのでnullを返す
            return null;
        }
        else
        {
            // Queueから1個取り出し返す
            var cherry = queueCherry.Dequeue();
            cherry.SetActive(true);

            return cherry;
        }
    }
    #endregion

    #region GeneratePeach - もも生成処理
    public GameObject GeneratePeach()
    {
        // Queueの中身を確認
        if (queuePeach.Count <= 0)
        {
            // Queueが空なのでnullを返す
            return null;
        }
        else
        {
            // Queueから1個取り出し返す
            var peach = queuePeach.Dequeue();
            peach.SetActive(true);

            return peach;
        }
    }
    #endregion

    #region GenerateGrape - ぶどう生成処理
    public GameObject GenerateGrape()
    {
        // Queueの中身を確認
        if (queueGrape.Count <= 0)
        {
            // Queueが空なのでnullを返す
            return null;
        }
        else
        {
            // Queueから1個取り出し返す
            var grape = queueGrape.Dequeue();
            grape.SetActive(true);

            return grape;
        }
    }
    #endregion

    #region CollectFruits - 各フルーツ格納処理
    public void CollectFruits(GameObject fruits)
    {
        // 非アクティブ状態に
        fruits.SetActive(false);

        // デフォルトポジションへ戻す
        fruits.transform.position = defPos;

        // キューに格納するフルーツをタグで判断
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


    #region プライベート変数

    // 各フルーツのプレファブ格納用
    [SerializeField] GameObject prefabApple;    // りんご
    [SerializeField] GameObject prefabCherry;   // さくらんぼ
    [SerializeField] GameObject prefabPeach;    // もも
    [SerializeField] GameObject prefabGrape;    // ぶどう

    [SerializeField] int maxCount;              // プレファブ生成上限数

    // 生成された各フルーツを格納するキュー
    private Queue<GameObject> queueApple;
    private Queue<GameObject> queueCherry;
    private Queue<GameObject> queuePeach;
    private Queue<GameObject> queueGrape;

    private Vector3 defPos = new Vector3(15f, 10f, 0f); // 各フルーツのデフォルトポジション

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    private void Awake()
    {
        // 各フルーツを格納するキューの生成
        queueApple = new Queue<GameObject>();
        queueCherry = new Queue<GameObject>();
        queuePeach = new Queue<GameObject>();
        queueGrape = new Queue<GameObject>();

        // 各フルーツを生成
        for (var i = 0; i < maxCount; i++)
        {
            // 生成
            var apple = Instantiate(prefabApple, defPos, Quaternion.identity);
            var cherry = Instantiate(prefabCherry, defPos, Quaternion.identity);
            var peach = Instantiate(prefabPeach, defPos, Quaternion.identity);
            var grape = Instantiate(prefabGrape, defPos, Quaternion.identity);

            apple.SetActive(false);
            cherry.SetActive(false);
            peach.SetActive(false);
            grape.SetActive(false);

            // Queueに追加
            queueApple.Enqueue(apple);
            queueCherry.Enqueue(cherry);
            queuePeach.Enqueue(peach);
            queueGrape.Enqueue(grape);
        }
    }
    #endregion

    #endregion
    
}
