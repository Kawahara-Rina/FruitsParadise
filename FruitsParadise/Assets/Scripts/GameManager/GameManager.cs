/*
    GameManager.cs 

    ゲーム中のメインの処理を行うクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region パブリック変数

    public float[] lines = new float[5];  // プレイヤーのライン座標
    public float[] fruitsSpan;            // 各フルーツの生成スパン
    public float eSpan;                   // エネミーの生成スパン

    public bool isGameEnd;                // ゲームが終了したかどうかのフラグ

    #endregion


    #region プライベート変数

    const int minLine = 0;
    const int maxLine = 5;

    [SerializeField] float spanMin;        // 各フルーツ生成スパンの最小値
    [SerializeField] float spanMax;        // 各フルーツ生成スパンの最大値

    [SerializeField] AudioClip fallSE;     // オブジェクト落下時の効果音

    private float[] delta = new float[5];  // 各フルーツ・エネミーの

    private List<int> rNumbers = new List<int>();  // ランダムで取得した値を格納するリスト

    private Vector3 screenRightTop;     // 画面右上座標の取得用

    private AudioSource audioSource;    // オーディオソース取得用

    private FruitsGenerator fg;         // FruitsGenerator取得用
    EnemyGenerator eg;                  // EnemyGenerator取得用
    private Timer tm;                   // Timer取得用

    #endregion

    #region プライベート関数

    #region Awake - 初期化処理
    private void Awake()
    {
        // ゲームは終わっていない状態
        isGameEnd = false;

        // 各フルーツ生成スパンをランダムに決定
        fruitsSpan[Define.APPLE] = Random.Range(spanMin, spanMax);
        fruitsSpan[Define.CHERRY] = Random.Range(spanMin, spanMax);
        fruitsSpan[Define.PEACH] = Random.Range(spanMin, spanMax);
        fruitsSpan[Define.GRAPE] = Random.Range(spanMin, spanMax);

        // 各オブジェクトの
        for(int i=0; i<delta.Length; i++)
        {
            delta[i] = 0;
        }

        // 世界座標の取得
        screenRightTop = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0));

        // オーディオソース取得
        audioSource = GetComponent<AudioSource>();

        // 各コンポーネント取得
        fg = GetComponent<FruitsGenerator>();
        eg = GetComponent<EnemyGenerator>();
        tm = GameObject.Find("timerImage").GetComponent<Timer>();
    }
    #endregion

    #region GenerateObject - 各オブジェクト生成処理
    /// <summary>
    /// 
    /// </summary>
    /// <param name="generateFunc">生成したいオブジェクトの生成関数呼び出しに使用</param>
    private void GenerateObject(GameObject generateFunc)
    {
        // 落下時の効果音を鳴らす
        audioSource.PlayOneShot(fallSE);

        // 各オブジェクト生成の関数を呼び出し、オブジェクトを生成
        var obj = generateFunc;

        if (obj != null)
        {
            // オブジェクト生成位置をランダムに決定
            var line = Random.Range(minLine, maxLine);
            var x = lines[line];
            var y = Random.Range(screenRightTop.y - 0.5f, screenRightTop.y - 2);

            obj.transform.position = new Vector3(x, y, 0);
        }
    }
    #endregion

    #region GenerateJudge - 生成の条件を満たしたオブジェクトを生成
    private void GenerateJudge()
    {
        // りんご生成処理
        if (delta[Define.APPLE] > fruitsSpan[Define.APPLE])
        {
            delta[Define.APPLE] = 0;
            GenerateObject(fg.GenerateApple());
        }

        // さくらんぼ生成処理
        if (delta[Define.CHERRY] > fruitsSpan[Define.CHERRY])
        {
            delta[Define.CHERRY] = 0;
            GenerateObject(fg.GenerateCherry());
        }

        // もも生成処理
        if (delta[Define.PEACH] > fruitsSpan[Define.PEACH])
        {
            delta[Define.PEACH] = 0;
            GenerateObject(fg.GeneratePeach());
        }

        // ぶどう生成処理
        if (delta[Define.GRAPE] > fruitsSpan[Define.GRAPE])
        {
            delta[Define.GRAPE] = 0;
            GenerateObject(fg.GenerateGrape());
        }

        // エネミー生成処理
        if (delta[Define.ENEMY] > eSpan)
        {
            delta[Define.ENEMY] = 0;
            GenerateObject(eg.GenerateEnemy());
        }
    }

    #endregion

    #region DeltaCountUp - 各オブジェクト生成までの経過時間をカウントアップ
    private void DeltaCountUp()
    {
        // 各オブジェクトの生成までの経過時間をカウントアップ
        for (int i = 0; i < delta.Length; i++)
        {
            delta[i] += Time.deltaTime;
        }

    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // 制限時間内であれば
        if (!isGameEnd && tm.isGameStart)
        {
            // 各オブジェクト生成までの経過時間をカウントアップ
            DeltaCountUp();
        }

        // 各オブジェクトの生成スパンを超えた場合に
        // 各オブジェクトを画面中に表示
        GenerateJudge();
    }
}
