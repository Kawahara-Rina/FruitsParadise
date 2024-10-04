/*
    ResultManager.cs

    リザルト処理を行うクラス
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    #region パブリック変数

    // 各スコアのテキスト
    public Text scoreText;               // 獲得スコアのテキスト
    public Text scoreTotalText;          // 獲得フルーツ数のテキスト
    public Text highscoreUpdateText;     // ハイスコア更新時のテキスト
    public Text scoreAppleText;          // りんごの獲得数のテキスト
    public Text scoreCherryText;         // さくらんぼの獲得数のテキスト
    public Text scorePeachText;          // ももの獲得数のテキスト
    public Text scoreGrapeText;          // ぶどうの獲得数のテキスト

    public static int rHighScore = 300;  // リザルト画面で表示するハイスコア

    #endregion

    #region パブリック関数

    #region TitleButtonOnClick - タイトルボタン押下でタイトル画面に遷移
    public void TitleButtonOnClick()
    {
        // タイトル画面に遷移
        SceneManager.LoadScene("TitleScene");
    }
    #endregion

    #region RetryButtonOnClick - リトライボタン押下でゲーム画面に遷移
    public void RetryButtonOnClick()
    {
        // ゲーム画面に遷移
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    #endregion


    #region プライベート変数

    // 上から落とす各フルーツのプレファブ格納用
    [SerializeField] GameObject prefabApple;    // りんご
    [SerializeField] GameObject prefabCherry;   // さくらんぼ
    [SerializeField] GameObject prefabPeach;    // もも
    [SerializeField] GameObject prefabGrape;    // ぶどう

    [SerializeField] int randomX;               // ランダムで生成する整数の範囲格納用
    [SerializeField] int startY;                // ランダムで生成するy座標の最小値
    [SerializeField] int endY;                  // ランダムで生成するy座標の最大値

    [SerializeField] float countSpeed;          // スコアをカウントするスピード
    [SerializeField] float maxY;                // y座標の上限値

    private float scoreCount;                   // スコアのカウントアップ表示に使用
    
    private bool isCountSkip;                   // スコアのカウントアップをスキップしたかどうかのフラグ

    private Vector3 defPos;                     // 各フルーツ生成のデフォルトポジション

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    private void Awake()
    {
        // rHighScoreに前回のゲームで獲得したハイスコアを格納
        rHighScore = PlayerManager.highScore;

        // ハイスコア更新のテキストを非表示
        highscoreUpdateText.enabled = false;

        // スコアを0から1ずつ増やしながら表示するため、0で初期化
        scoreCount = 0;

        // スコア表示をスキップされていない状態
        isCountSkip = false;

        // 各フルーツ生成のデフォルトポジション
        defPos = new Vector3(0f, 8f, 0f);

        // それぞれのフルーツを獲得数分落下させる
        FruitsFall(PlayerManager.getApple, prefabApple);
        FruitsFall(PlayerManager.getCherry, prefabCherry);
        FruitsFall(PlayerManager.getPeach, prefabPeach);
        FruitsFall(PlayerManager.getGrape, prefabGrape);
    }
    #endregion

    #region ShowHighScoreText - ハイスコアが更新されていたらスコア更新のテキストを表示
    private void ShowHighScoreText()
    {
        // ハイスコアが更新されていた場合
        if (PlayerManager.isHighScoreUpdate)
        {
            // スコア更新のテキストを表示する
            highscoreUpdateText.enabled = true;
        }
    }

    #endregion

    #region ShowScore - スコアカウントアップ表示スキップ時に表示するスコア
    private void ShowScore()
    {
        // それぞれのスコアを一度に表示する
        scoreText.text = string.Format("{0}", PlayerManager.score);
        scoreTotalText.text = string.Format("{0}", PlayerManager.fruitsScore);
        scoreAppleText.text = string.Format("× {0}", PlayerManager.getApple);
        scoreCherryText.text = string.Format("× {0}", PlayerManager.getCherry);
        scorePeachText.text = string.Format("× {0}", PlayerManager.getPeach);
        scoreGrapeText.text = string.Format("× {0}", PlayerManager.getGrape);
    }
    #endregion

    #region SkipShowScore - スコアカウントアップ表示がスキップされた場合に、一度に値を表示する
    // スコアのカウントアップ表示がスキップされた場合に、一気に値を表示する関数
    private void SkipShowScore()
    {
        // スキップされていない場合
        if (isCountSkip == false)
        {
            // タップされたら
            if (Input.GetMouseButtonDown(0))
            {
                // スキップのフラグを立てる
                isCountSkip = true;
            }
        }

        // スキップされた場合
        else
        {
            // ハイスコアが更新されていたら更新の文字を表示する
            ShowHighScoreText();

            // それぞれのスコアを一気に表示する
            ShowScore();
        }
    }

    #endregion

    #region FruitsFall - フルーツが上から降ってくる演出
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fruits">生成するフルーツの数</param>
    /// <param name="prefab">生成するプレファブ</param>
    void FruitsFall(int fruits, GameObject prefab)
    {
        // 獲得したフルーツの数分繰り返し、プレファブを生成する
        for (var i = 0; i < fruits; i++)
        {
            // プレファブを生成する座標のx座標はまばらに
            // randomX : ランダムの範囲を指定した変数
            var x = Random.Range(-randomX, randomX);

            // フルーツの獲得数分プレファブを生成生成
            Instantiate(prefab, defPos, Quaternion.identity);

            // 生成する位置のx座標はランダムで取得した値
            // 生成する位置のy座標は少しずつ落ちてくるように上から
            defPos.x = x;
            defPos.y += i;       

            // 生成する位置のy座標が大きくなりすぎた場合
            if (defPos.y >= maxY)
            {
                // y座標の位置を低めに調整
                var y = Random.Range(startY, endY);
                defPos.y = y;
            }
        }
    }

    #endregion

    #region ScoreCount - スコア(リザルト)を１ずつ増やしながら表示する
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_score">どのスコアかを指定</param>
    /// <param name="_text">どのテキストに表示するかを指定</param>
    /// <param name="fruit">スコアはフルーツか、総合スコアかを指定</param>
    void ScoreCount(int _score, Text _text, bool fruit)
    {
        // スキップされていない場合
        if (isCountSkip == false)
        {
            //スコアを１ずつ増やしながら表示する
            if (scoreCount < _score)
            {
                // フルーツ獲得数表示の場合
                if (fruit)
                {
                    // フルーツ ×　獲得数　　と表示
                    _text.text = string.Format("× {0}", (int)scoreCount);
                }
                // スコア表示の場合
                else
                {
                    // scoreCountを少しずつ加算
                    scoreCount += countSpeed;

                    // スコアを表示
                    _text.text = string.Format("{0}", (int)scoreCount);
                }
            }
            // スコアの加算表示を終えた場合
            else
            {
                if (fruit)
                {
                    // 獲得数を表示
                    _text.text = string.Format("× {0}", _score);
                }
                else
                {
                    // スコアを表示
                    _text.text = string.Format("{0}",_score);
                }
            }
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // スコアのカウントアップ表示がスキップされた場合の処理
        SkipShowScore();

        // スコア、各フルーツ獲得数をカウントアップ表示
        ScoreCount(PlayerManager.score, scoreText, false);
        ScoreCount(PlayerManager.fruitsScore, scoreTotalText, false);
        ScoreCount(PlayerManager.getApple, scoreAppleText, true);
        ScoreCount(PlayerManager.getCherry, scoreCherryText, true);
        ScoreCount(PlayerManager.getPeach, scorePeachText, true);
        ScoreCount(PlayerManager.getGrape, scoreGrapeText, true);
    }
}
