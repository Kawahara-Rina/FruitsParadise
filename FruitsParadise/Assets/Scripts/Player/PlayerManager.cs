/*
    PlayerManager.cs

    プレイヤーの動きを管理するクラス。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    #region パブリック変数

    public static int fruitsScore;  // フルーツ獲得総数
    public static int score;        // スコア
    public static int highScore;    // ハイスコア

    // 各フルーツの獲得数
    public static int getApple;     // りんご
    public static int getCherry;    // さくらんぼ
    public static int getGrape;     // ぶどう
    public static int getPeach;     // もも

    public static bool isHighScoreUpdate;  // ハイスコアを更新したかどうかのフラグ
    public bool isFreeze;                  // プレイヤーがフリーズしているかどうかのフラグ

    #endregion


    #region プライベート変数

    // 移動方向
    private enum OperationType
    {
        RIGHT,
        LEFT,
        NONE
    }

    [SerializeField] Text scoreText;       // スコアを表示するテキスト
    [SerializeField] Text getText;         // フルーツゲット時に表示するテキスト
    [SerializeField] Image getImage;       // フルーツゲット時に表示する画像

    [SerializeField] AudioClip getSE;      // フルーツゲット時の効果音
    [SerializeField] AudioClip enemyGetSE; // エネミーに触れた場合の効果音

    [SerializeField] float speed;          // 移動スピード
    [SerializeField] float[] getTextPos = new float[5];  // getTextの座標

    private int line;     // 現在のライン
    private int nextLine; // 移動後のライン

    private float textDeleteTimer; // テキストを一定時間で消すためのタイマー

    private bool isMove;  // 移動するかどうかのフラグ
    private bool isGet;   // 何かをゲットしたかどうかのフラグ


    private GameManager gm;       // GameManager取得用
    private Timer tm;             // Timer取得用
    private FruitsGenerator fg;   // FruitsGenerator取得用
    private EnemyGenerator eg;    // EnemyGenerator取得用
    private Frame fr;             // Frame取得用

    private AudioSource audioSource;     // オーディオソース取得用
    private Animator pAnimator;          // プレイヤーのアニメーター取得用    
    private Animator getTextAnimator;    // getTextのアニメーター取得用
    private Animator getImageAnimator;   // getImageのアニメーター取得用

    #endregion

    #region プライベート関数

    #region OnTriggerEnter2D - クモが触れた場合の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 触れた物のタグを取得
        var tag = collision.gameObject.tag;

        // 触れたものがクモであれば
        if (tag == "Spider")
        {
            // プレイヤーのフリーズフラグを立てる、フリーズ時のアニメーション
            isFreeze = true;
            pAnimator.SetTrigger("Freeze");

            // 効果音を鳴らす
            audioSource.PlayOneShot(enemyGetSE);

            // クモを定位置に戻す
            eg.CollectEnemy(collision.gameObject);
        }
    }
    #endregion

    #region OnCollisionEnter2D - フルーツが衝突した場合の処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 触れたオブジェクトをキューに格納、定位置に戻す
        fg.CollectFruits(collision.gameObject);

        // 各フルーツをゲットしたフラグを立てる
        isGet = true;

        // ゲット時の効果音を鳴らす
        audioSource.PlayOneShot(getSE);

        // テキストのアニメーション、トリガー発火
        getTextAnimator.SetTrigger("Get");
        // イメージのアニメーション、トリガー発火
        getImageAnimator.SetTrigger("Get");

        // tag : 衝突したコリジョンに付けられた各フルーツのタグを取得
        var fruitsTag = collision.gameObject.tag;

        // 衝突したフルーツのタグを見て、各スコアを加算する関数を実行
        ScoreAddition(fruitsTag, collision);
    }
    #endregion

    #region Awake - 初期化関数
    private void Awake()
    {
        // 各スコアの初期化
        fruitsScore = 0;
        score = 0;
        highScore = TitleManager.tHighScore;
        getApple = 0;
        getCherry = 0;
        getPeach = 0;
        getGrape = 0;

        // ハイスコア更新していない状態
        isHighScoreUpdate = false;
        // フリーズしていない状態
        isFreeze = false;
        // 移動していない状態
        isMove = false;

        // 現在プレイヤーのいるライン 初期位置 中央
        line = 2;
        // 移動後のライン
        nextLine = 2;

        // タイマーの初期秒数
        textDeleteTimer = 0;

        // 各スクリプト取得
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        fg = GameObject.Find("GameManager").GetComponent<FruitsGenerator>();
        eg = GameObject.Find("GameManager").GetComponent<EnemyGenerator>();
        fr = GameObject.Find("frameImage").GetComponent<Frame>();
        tm = GameObject.Find("timerImage").GetComponent<Timer>();

        // オーディオソース取得
        audioSource = GetComponent<AudioSource>();

        // プレイヤーのアニメーター取得
        pAnimator = GetComponent<Animator>();
        // getTextのアニメーター取得
        getTextAnimator = GameObject.Find("getText").GetComponent<Animator>();
        // getImageのアニメーター取得
        getImageAnimator = GameObject.Find("getImage").GetComponent<Animator>();
    }
    #endregion

    #region ScoreAddition - 衝突した物のタグを見て、各フルーツのスコアを加算する
    private void ScoreAddition(string fruitsTag, Collision2D collision)
    {

        // フルーツの総獲得数を1増やす
        fruitsScore++;

        switch (fruitsTag)
        {
            // りんごの場合
            case Define.TAG_APPLE:

                // りんごの獲得数を1増やす
                getApple++;

                // 現在選ばれているフルーツもりんごの場合は獲得時にスコアを増やす
                if (fr.sFruits == Define.APPLE)
                {
                    score += 30;
                }
                // りんご以外の場合は通常スコア
                else
                {
                    score += 10;
                }

                break;

            // さくらんぼの場合
            case Define.TAG_CHERRY:

                // さくらんぼの獲得数を1増やす
                getCherry++;

                // 現在選ばれているフルーツもさくらんぼの場合は獲得時にスコアを増やす
                if (fr.sFruits == Define.CHERRY)
                {
                    score += 30;
                }
                // さくらんぼ以外の場合は通常スコア
                else
                {
                    score += 10;
                }

                break;

            // ぶどうの場合
            case "Grape":

                // ぶどうの獲得数を1増やす
                getGrape++;

                // 現在選ばれているフルーツもぶどうの場合は獲得時にスコアを増やす
                if (fr.sFruits == Define.GRAPE)
                {
                    score += 30;
                }
                // ぶどう以外の場合は通常スコア
                else
                {
                    score += 10;
                }

                break;

            // ももの場合
            case "Peach":

                // りんごの獲得数を1増やす
                getPeach++;

                // 現在選ばれているフルーツもももの場合は獲得時にスコアを増やす
                if (fr.sFruits == Define.PEACH)
                {
                    score += 30;
                }
                // もも以外の場合は通常スコア
                else
                {
                    score += 10;
                }

                break;
        }
    }
    #endregion

    #region ShowGetObject - フルーツゲット時にテキストやイメージを表示する
    private void ShowGetObjects()
    {
        // getTextの表示
        getText.enabled = true;
        // getImageの表示
        getImage.enabled = true;

        // getTextの位置を、プレイヤーの今いる座標に
        getText.transform.localPosition = new Vector3(getTextPos[nextLine], -250, 0);
        // getImageの位置を、プレイヤーの今いる座標に
        getImage.transform.localPosition = new Vector3(getTextPos[nextLine] - 105, -181, 0);

        // text,Imageを数秒表示するために、カウントアップ
        textDeleteTimer += Time.deltaTime;

        // タイマーで一定秒経過
        // またはその場から動くとgetText,Imageを非表示
        if (textDeleteTimer >= 1.5f || isMove)
        {
            // textを非表示にする
            getText.enabled = false;
            // imageを非表示にする
            getImage.enabled = false;

            isGet = false;

            // 非表示にするまでのタイマーの初期化
            textDeleteTimer = 0;
        }
    }
    #endregion

    #region ShowFruitsScore - フルーツの総獲得数を表示
    private void ShowFruitsScore()
    {
        // フルーツの総獲得数を表示
        scoreText.text = string.Format("{0}", fruitsScore);
    }
    #endregion

    #region JudgeHighScore - ハイスコアを更新したか判断
    private void JudgeHighScore()
    {
        // スコアがハイスコアを超えたらハイスコアにする
        if (TitleManager.tHighScore < score)
        {
            // ハイスコアが更新されたフラグを立てる
            isHighScoreUpdate = true;
            highScore = score;
        }
    }
    #endregion

    #region GetDirection - キー入力が何かを判断し、移動方向を返す関数
    private OperationType GetDirection()
    {
        // ret 入力方向を返す値
        OperationType ret = OperationType.NONE;

        // 右矢印キーが入力された場合
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // retに右方向移動を格納
            ret = OperationType.RIGHT;
        }
        // 左矢印キーが入力された場合
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // retに左方向移動を格納
            ret = OperationType.LEFT;
        }
        else
        {
            // 上記意外は移動しない
            ret = OperationType.NONE;
        }

        // 移動方向を返す
        return ret;
    }
    #endregion

    #region DirectionCheck - 移動方向チェック処理
    private void DirectionCheck()
    {
        // direction : GetDirection関数で返された値(各OperationType)を格納
        var direction = GetDirection();

        // directionの値により処理を変える
        switch (direction)
        {
            // OperationType.RIGHT : 右方向にスワイプされた場合
            // プレイヤーの今いるラインが右端以外の場合、次のラインを1増やし、右に移動
            case OperationType.RIGHT:
                if (line < gm.lines.Length - 1)
                {
                    nextLine = line + 1;
                }
                isMove = true;
                break;

            // OperationType.LEFT : 左方向にスワイプされた場合
            // プレイヤーの今いるラインが左端以外の場合、次のラインを1減らし、左に移動
            case OperationType.LEFT:
                if (line > 0)
                {
                    nextLine = line - 1;
                }
                isMove = true;
                break;

            default:
                break;
        }
    }
    #endregion

    #region Move - プレイヤー移動処理
    private void Move()
    {
        // pos : プレイヤーの現在座標を格納
        var pos = transform.position;

        // 右に移動の場合
        if (line < nextLine)
        {
            // xをプラス方向に移動
            transform.position += new Vector3(speed, 0, 0);

            // 次のラインまで移動したら止める
            if (transform.position.x >= gm.lines[nextLine])
            {
                // 現在のラインを移動後のラインへ
                line = nextLine;
                isMove = false;

                // 移動時に、少し座標のずれが生じる場合があるためgm.linesに格納された座標に配置しなおす
                transform.position = new Vector3(gm.lines[line], pos.y, pos.z);
            }
        }
        // 左に移動の場合
        else if (line > nextLine)
        {
            // xをマイナス方向に移動
            transform.position += new Vector3(-speed, 0, 0);

            // 次のラインまで移動したら止める
            if (transform.position.x <= gm.lines[nextLine])
            {
                // 現在のラインを移動後のラインへ
                line = nextLine;
                isMove = false;

                // 移動時に、少し座標のずれが生じる場合があるためgm.linesに格納された座標に配置しなおす
                transform.position = new Vector3(gm.lines[line], pos.y, pos.z);
            }
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // フルーツの総獲得数を表示
        ShowFruitsScore();

        // ゲーム中、フリーズ指定ない場合はキー入力を受け付ける
        if (tm.isGameStart && !isFreeze)
        {
            // 移動方向のチェック
            DirectionCheck();
        }

        // 移動フラグが成立している場合
        if (isMove)
        {
            // プレイヤー移動処理の関数を実行
            Move();
        }

        // 各フルーツをゲットした場合
        if (isGet)
        {
            // ゲット時のテキストやイメージを表示する関数を実行
            ShowGetObjects();
        }

        // ハイスコアを更新したかを判断する関数を実行
        JudgeHighScore();
    }
}
