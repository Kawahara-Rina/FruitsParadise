/*
    TitleManager.cs

    タイトル画面での処理を行うクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{
    #region パブリック変数

    public static int tHighScore;  // タイトル画面で表示するハイスコア

    #endregion

    #region パブリック関数

    #region StartButtonOnClick - スタートボタン押下でゲームスタートのフラグを立てる
    public void StartButtonOnClick()
    {
        // スタートのフラグを立てる
        isStart = true;
    }
    #endregion

    #region NextButtonOnClick - 遊び方説明の次へボタン押下で次の画像に切り替え
    public void NextButtonOnClick()
    {
        // 遊び方説明が最後のページでなければ次へ進む
        if (imageNumber != ruleImages.Length - 1)
        {
            // 配列の添え字を次へ
            imageNumber++;
        }

        // 次の画像を表示
        ruleImages[imageNumber].enabled = true;

        // ひとつ前の画像を非表示
        ruleImages[imageNumber - 1].enabled = false;
    }
    #endregion

    #region BackButtonOnClick - 遊び方説明の戻るボタン押下で前の画像に切り替え
    public void BackButtonOnClick()
    {
        // 遊び方説明が最初のページでなければひとつ前へ戻る
        if (imageNumber != 0)
        {
            // 配列の添え字を一つ前へ
            imageNumber--;
        }
        // 次の画像を表示
        ruleImages[imageNumber].enabled = true;

        // ひとつ前の画像を非表示に
        ruleImages[imageNumber + 1].enabled = false;
    }
    #endregion

    #region HowtoPlayButtonOnClick - 遊び方ボタン押下で遊び方説明を表示
    public void HowtoPlayButtonOnClick()
    {
        // あそびかた説明を表示
        ruleImages[imageNumber].enabled = true;
    }
    #endregion

    #region ExitButtonOnClick - 遊び方説明を閉じるボタン押下で遊び方説明を非表示
    public void ExitButtonOnClick()
    {
        // 遊び方説明を非表示
        ruleImages[imageNumber].enabled = false;
    }
    #endregion

    #endregion


    #region プライベート変数

    [SerializeField] Text highScoreText;                // ハイスコアのテキスト
    [SerializeField] Image[] ruleImages = new Image[4]; // ルール説明の画像格納用配列
    [SerializeField] GameObject rabbitImage;            // うさぎの画像格納用

    [SerializeField] float wait;       // 画面遷移時数秒待つ場合に使用

    [SerializeField] AudioClip tapSE;  // タップ時の効果音

    private int imageNumber;           // 配列(ruleImages)の添え字変更、指定に使用
    private bool isStart;              // スタートボタンが押されたかどうかのフラグ

    private AudioSource audioSource;   // AudioSource取得用
    private Animator rAnimator;        //　うさぎのアニメーター取得用

    #endregion

    #region プライベート関数

    #region Awake - 初期化関数
    private void Awake()
    {
        // tHighScoreに リザルト画面で最後に記録されたハイスコアを格納
        tHighScore = ResultManager.rHighScore;

        // ハイスコアを表示
        highScoreText.text = string.Format("ハイスコア：{0}", tHighScore);

        // 配列の添え字用変数の初期化
        imageNumber = 0;

        // 配列(ruleImage)の各要素を非アクティブで初期化
        for (int i = 0; i < ruleImages.Length; i++)
        {
            ruleImages[i].enabled = false;
        }

        // ゲームはスタートしていない状態
        isStart = false;

        // オーディオソースを取得
        audioSource = GetComponent<AudioSource>();
        // うさぎのアニメーターを取得
        rAnimator = GameObject.Find("rabbitImage").GetComponent<Animator>();
    }
    #endregion

    #region ToGameScene - GameSceneへ遷移する関数
    private void ToGameScene()
    {
        //ゲームシーンへ遷移
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    #region RabbitAnimation - 画面タップ時にうさぎのアニメーションをする関数
    private void RabbitAnimation()
    {
        // タップ時効果音を鳴らす
        audioSource.PlayOneShot(tapSE);

        // Rayを発射して、画面をタップしたかどうか調べる
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        // Rayで何もヒットしなかった場合は 画面がタップされた
        // ヒットした場合でもUIオブジェクトでない場合
        if (!hit2d || hit2d.transform.gameObject.tag != "UIObject")
        {
            // 現在のうさぎのアニメーションが動いている状態でなければ
            if (!rAnimator.GetCurrentAnimatorStateInfo(0).IsName("move@titleRabbit"))
            {
                // Tapトリガーを発火、動いているアニメーションへ遷移
                rAnimator.SetTrigger("Tap");
            }
        }
    }
    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        // スタートボタンが押された場合
        if (isStart)
        {
            // 2秒後にゲームシーンに遷移する関数を実行
            Invoke("ToGameScene", wait);
        }

        // クリックされた場合
        if (Input.GetMouseButtonDown(0))
        {
            // うさぎのアニメーションをする関数を実行
            RabbitAnimation();
        }

    }
}
