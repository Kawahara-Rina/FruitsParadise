/*
    FruitsManager.cs

    各フルーツを管理するクラス。 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitsManager : MonoBehaviour
{
    #region プライベート変数

    private Vector3 screenLeftBottom;   // 画面左下の座標格納用

    private FruitsGenerator fg;         // FruitsGenerator取得用

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    private void Awake()
    {
        // 画面の左下の座標を取得
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // FruitsGenerator取得
        fg = GameObject.Find("GameManager").GetComponent<FruitsGenerator>();
    }
    #endregion

    #endregion


    // Update is called once per frame
    void Update()
    {
        // 画面の一番下よりy座標が小さくなったオブジェクトを格納
        if (transform.position.y < screenLeftBottom.y - 1f)
        {
            fg.CollectFruits(gameObject);
        }
    }
}
