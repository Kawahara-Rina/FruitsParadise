/*
    EnemyManager.cs

    敵の動きなどを制御するクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region プライベート変数

    private Vector3 screenLeftBottom;    // 画面左下の座標取得用
    private EnemyGenerator eg;           // EnemyGenerator取得用

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
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
        // 画面の一番下よりy座標が小さくなったオブジェクトを格納
        if (transform.position.y < screenLeftBottom.y - 1f)
        {
            eg.CollectEnemy(gameObject);
        }
    }
}
