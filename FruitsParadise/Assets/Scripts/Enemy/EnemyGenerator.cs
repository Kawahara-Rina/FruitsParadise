/*
    EnemyGenerator.cs

    敵を生成するクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    #region パブリック関数

    #region GenerateEnemy - エネミー生成処理
    public GameObject GenerateEnemy()
    {
        // Queueの中身を確認
        if (queueEnemy.Count <= 0)
        {
            // Queueが空なのでnullを返す
            return null;
        }
        else
        {
            // Queueから1個取り出す
            var enemy = queueEnemy.Dequeue();
            enemy.SetActive(true);

            return enemy;
        }
    }
    #endregion

    #region CollectEnemy - エネミー格納処理
    public void CollectEnemy(GameObject enemy)
    {
        // 非アクティブ状態に
        enemy.SetActive(false);

        // デフォルトポジションへ戻す
        enemy.transform.position = defPos;

        // Queueに格納
        queueEnemy.Enqueue(enemy);
    }
    #endregion

    #endregion


    #region プライベート変数

    [SerializeField] GameObject prefabEnemy;     // エネミーのプレファブ
    [SerializeField] int maxCount;               // 生成上限数

    private Queue<GameObject> queueEnemy;                // 生成したエネミーのオブジェクトを格納するキュー
    private Vector3 defPos = new Vector3(20f, 10f, 0f);  // デフォルトポジション

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    // 初期化処理
    private void Awake()
    {
        // キューを生成
        queueEnemy = new Queue<GameObject>();

        // エネミーを生成
        for (var i = 0; i < maxCount; i++)
        {
            // 生成
            var enemy = Instantiate(prefabEnemy, defPos, Quaternion.identity);
            enemy.SetActive(false);

            // Queueに追加
            queueEnemy.Enqueue(enemy);
        }
    }
    #endregion

    #endregion

}
