/*
    Timer.cs

    制限時間の管理を行うクラス。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    #region パブリック変数

    public float timer;            // 制限時間用のタイマー
    public bool isGameStart;       // ゲームが始まったかどうかのフラグ

    #endregion

    #region パブリック関数

    #region pauseButtonOnClick - ポーズボタン押下でゲームを一時停止
    public void pauseButtonOnClick(){


    }
    #endregion

    #endregion


    #region プライベート変数

    [SerializeField] Text TimerText;         // タイマーのテキスト指定用
    [SerializeField] Text StartTimerText;    // ゲーム開始時のカウントダウンのテキスト指定用
    [SerializeField] GameObject timeupPanel; // タイムアップ時に表示するパネル指定用

    [SerializeField] AudioClip countSE;      // カウント時の効果音
    [SerializeField] AudioClip startSE;      // ゲームスタート時の効果音
    [SerializeField] AudioClip endSE;        // ゲーム終了時の効果音

    [SerializeField] float lastSpurtTime;    // ラストスパートの秒数
    [SerializeField] float lsEnemy;          // ラストスパート時のエネミーの生成スパン
    [SerializeField] float lsMinFruits;      // ラストスパート時のフルーツの生成スパン、ランダムの最小値
    [SerializeField] float lsMaxFruits;      // ラストスパート時のエネミーの生成スパン、ランダムの最大値


    private int seOnceCount = 0;       // 効果音を一度だけ鳴らすために使用する変数
    private int lastSpurtCount ;       // ラストスパートで使うカウント

    private float countSETimer = 0;    // countSEを1秒に1回鳴らすために使用する変数
    private float startTimer;          // ゲーム開始時のカウントダウンのタイマー
   
    private GameManager gm;            // GameManager取得
    private Animator animator;         // Animator取得
    private AudioSource audioSource;   // AudioSource取得

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理 
    private void Awake()
    {
        // ゲームスタートしていない状態
        isGameStart = false;

        // ラストスパートに入ったかのカウント
        lastSpurtCount = 0;

        // ゲーム開始時のタイマー初期値
        startTimer = 3;

        // タイムアップパネルは非表示
        timeupPanel.SetActive(false);

        // 各コンポーネント取得(GameManager,Animator,AudioSource)
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region TimeLimit - ゲーム中の制限時間の処理
    private void TimeLimit()
    {
        // タイマーを表示する(小数点以下切り捨て表示)
        // Mathf.Ceil(timer) → timerの小数点を切り捨てる
        TimerText.text = string.Format("{0}", Mathf.Ceil(timer));

        // 制限時間を1秒ずつカウントダウン
        timer -= Time.deltaTime;

        // 制限時間が近づいてきたらタイマーを点滅
        if (timer <=10)
        {
            // アニメーションのBlinkトリガーをセット
            animator.SetTrigger("Blink");
        }

        // タイマーがマイナスの値(0)になった場合
        if (timer < 0)
        {
            // ゲーム終了のフラグを立てる
            gm.isGameEnd = true;
            
            // ゲーム終了時の効果音を鳴らす
            OnceSound(endSE);
            
            // タイムアップパネルを表示する
            timeupPanel.SetActive(true);

            // タイマーは0と表示
            TimerText.text = string.Format("0");

            // アニメーションのIdleトリガーを発火
            animator.SetTrigger("Idle");
            
            // ゲーム終了から数秒経過でリザルト画面へ遷移
            if (timer < -3)
            {
                SceneManager.LoadScene("ResultScene");
            }
        }
    }
    #endregion

    #region LastSpurt - ラストスパート時に各プレファブの生成スパンを短く設定
    private void LastSpurt()
    {
        // 一度だけランダムの値を代入
        if (lastSpurtCount == 0)
        {
            // クモの出現スパンを短く設定
            gm.eSpan = lsEnemy;

            // フルーツの出現スパンを短く設定
            gm.fruitsSpan[Define.APPLE] = Random.Range(lsMinFruits, lsMaxFruits);
            gm.fruitsSpan[Define.CHERRY] = Random.Range(lsMinFruits, lsMaxFruits);
            gm.fruitsSpan[Define.PEACH] = Random.Range(lsMinFruits, lsMaxFruits);
            gm.fruitsSpan[Define.GRAPE] = Random.Range(lsMinFruits, lsMaxFruits);

            lastSpurtCount++;
        }
    }
    #endregion

    #region StartWaitTime - ゲームを開始するまで数秒待つ処理
    public void StartWaitTime()
    {
        //isGameStart = false;

        //startTimer = 3;
        // スタート時のテキストを非表示にする
        StartTimerText.enabled = true;

        // 1秒に一回カウントのSEを鳴らす
        CountSound();

        // タイマーを表示
        TimerText.text = string.Format("{0}", Mathf.Ceil(timer));

        // カウントダウンタイマーを表示する(小数点以下を切り捨て表示)
        // Mathf.Ceil(timer) → timerの小数点を切り捨てる
        StartTimerText.text = string.Format("{0}", Mathf.Ceil(startTimer));

        // スタート時のタイマーを1秒ずつカウントダウン
        startTimer -= Time.deltaTime;

        // タイマーが0になったら
        if (startTimer <= 0)
        {
            // ゲームスタート時の効果音を一度だけ鳴らす
            OnceSound(startSE);

            // テキストにゲーム開始の文字列を代入
            StartTimerText.text = string.Format("START !!");

            // ゲームスタートしてから数秒経ったならゲームを開始する
            if (startTimer < -1.3f)
            {
                // 効果音を一度だけ鳴らすカウントを0で初期化
                seOnceCount = 0;

                startTimer = 3;
                // スタート時のテキストを非表示にする
                StartTimerText.enabled = false;
                //pauseButton.SetActive(true);

                // ゲームスタートのフラグを立てる
                isGameStart = true;
            }
        }
    }
    #endregion

    #region CountSound - countSEを1秒に一回鳴らす処理
    private void CountSound()
    {
        // 3,2,1のカウント音を鳴らす
        // 1秒に一回鳴らす
        if (startTimer > 0)
        {
            countSETimer -= Time.deltaTime;
        }

        if (countSETimer <= 0)
        {
            audioSource.PlayOneShot(countSE);
            countSETimer = 1;
        }
    }
    #endregion

    #region OnceSound - 指定した効果音を一度だけ鳴らす処理
    // 効果音を一度だけ鳴らす処理
    private void OnceSound(AudioClip se)
    {
        // 一度だけ音を鳴らす
        if (seOnceCount == 0)
        {
            // 指定した効果音を一度だけ鳴らす
            audioSource.PlayOneShot(se);
            seOnceCount++;
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // ゲーム中の処理
        if (isGameStart)
        {
            // 制限時間の関数を実行
            TimeLimit();

            // ラストスパートに入った場合
            if (timer <= lastSpurtTime)
            {
                // ラストスパート時の処理を実行
                LastSpurt();
            }
        }
        // ゲーム開始前の処理
        else
        {
            // ゲーム開始までのカウントダウンの実行
            StartWaitTime();
        }
    }
}
