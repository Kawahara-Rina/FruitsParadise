/*
    Frame.cs

    プレイヤーの隣に表示される吹き出しの管理を行うクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    #region パブリック変数

    public int sFruits;      // ランダムに選ばれたフルーツを格納する変数

    #endregion


    #region プライベート変数

    [SerializeField] int callSeconds;  // 何秒ごとに関数を呼び出すかの秒数を格納する変数
    private Animator animator;         // アニメーター取得

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    private void Awake()
    {
        // アニメーターのコンポーネントを取得
        animator = GetComponent<Animator>();

        // ShowFrame関数をcallSeconds度に呼び出す
        InvokeRepeating("ShowFrame", 1, callSeconds);
    }
    #endregion

    #region SelectFruits - 各フルーツからランダムに一つ選ぶ
    private void SelectFruits()
    {
        // 各フルーツからランダムに選ぶ
        sFruits = Random.Range(Define.APPLE, Define.GRAPE);
    }
    #endregion

    #region ShowFrame - 選ばれたフルーツとふきだしを表示する
    private void ShowFrame()
    {
        // 各フルーツから一つ選ぶ
        SelectFruits();

        // 選ばれたフルーツは何かを判断
        // アニメーションのトリガーのセット(各ふきだしと、選ばれたフルーツが表示されるアニメーション)
        switch (sFruits)
        {
            // りんごの場合
            case Define.APPLE:
                animator.SetTrigger("fApple");
                break;

            // さくらんぼの場合
            case Define.CHERRY:
                animator.SetTrigger("fCherry");
                break;

            // ももの場合
            case Define.PEACH:
                animator.SetTrigger("fPeach");
                break;

            // ぶどうの場合
            case Define.GRAPE:
                animator.SetTrigger("fGrape");
                break;
        }
    }
    #endregion

    #endregion

}