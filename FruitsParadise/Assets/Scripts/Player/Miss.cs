/*
    Miss.cs

    フルーツを取り逃した際の処理を行うクラス。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miss : MonoBehaviour
{
    #region プライベート変数

    private Animator animator;  //アニメーターを取得

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    private void Awake()
    {
        // アニメーターのコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    #endregion

    #region OnTriggerEnter2D - フルーツが触れた場合テキストを表示(アニメーション)する
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 触れたオブジェクトのタグを取得
        var tag = collision.gameObject.tag;

        // 触れた物がクモ以外であれば(フルーツならば)
        if (tag != Define.TAG_ENEMY)
        {
            // アニメーションのトリガー Missをセット
            // フルーツを取れなかった場合、画面下にMISSを表示するアニメーション
            animator.SetTrigger("Miss");
        }
    }
    #endregion

    #endregion

}
