using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    // State を表示する
    public UnityEngine.UI.Text stateViewer;

    // プレーヤーのゲームオブジェクト
    public GameObject player0, player1;

    // エピソード数を表示するやつ
    public UnityEngine.UI.Text episodeCountText;

    // ステップ数を表示するやつ
    public UnityEngine.UI.Text stepCountText;

    // 勝数を表示するやつ
    public UnityEngine.UI.Text p0ScoreText, p1ScoreText, drawScoreText;

    // いわゆるスコアを表示するやつ
    public UnityEngine.UI.Text totalScoreText;

    // エージェントの愛称を表示するやつ
    public UnityEngine.UI.Text agent0Name, agent1Name;

    // 勝利表示の文字
    public UnityEngine.UI.Text p0WinText, p1WinText, drawText;

    // エピソード数
    public static int episodeCount = 0;

    // ステップ数
    int stepCount = 0;

    // 勝敗数
    static int p0Score, p1Score, drawScore;

    // いわゆるスコア
    static int totalScore;

    // プレーヤーを操作するエージェント
    public static Agent agent0, agent1;

    // 勝ち負けがいまどんなか
    State.Winner winner = State.Winner.Continue;

    // ゲームオーバーになった時刻
    float gameOverTime;

    // 早送りしてるかどうか
    static bool isFF = false;

    // Use this for initialization
    void Start()
    {
        // デバッグ用
        agent0 = agent0 ?? new SampleAgent(Agent.ID.P0);
        agent1 = agent1 ?? new SampleAgent(Agent.ID.P1);

        UpdateEpisodeCount();
        UpdateAgentName();

        agent0.OnEpisodeStart();
        agent1.OnEpisodeStart();

        InitialVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleFF();
        GameReset();
    }

    void FixedUpdate()
    {
        // Scene が複数読み込まれていたら何もしません
        if (SceneManager.sceneCount > 1)
        {
            return;
        }

        UpdateStepCount();

        // ゲームが終わったあとの処理はこの if 文の中でおわり
        if (winner != State.Winner.Continue)
        {
            // 早送りししてないとき resultDispSec 経過するまではリザルト画面
            if (!isFF && UnityEngine.Time.fixedTime - gameOverTime < Prefs.resultDispSec)
            {
                return;
            }

            // 新しいゲームの開始
            NewGame();
            return;
        }

        // ここから先はゲームが続いている場合の対処
        State s = new State(player0, player1);
        Action a0 = agent0.Step(s);
        Action a1 = agent1.Step(s);
        UpdateStateText(s, a0, a1);
        ActionPlayer(player0, a0);
        ActionPlayer(player1, a1);

        // ゲームが終わっているか確かめる
        winner = Judge(s);
        // 最大ステップ数を超えたら強制的に引き分け
        if (stepCount > Prefs.maxStepCount)
        {
            winner = State.Winner.Draw;
        }

        // スコア表の更新
        UpdateScoreText(winner);
        UpdateTotalScore(winner);

        // ゲームが終わった瞬間の対処（一度きりしかやらない）
        if (winner != State.Winner.Continue)
        {
            gameOverTime = UnityEngine.Time.fixedTime;

            // 買った方の名前を表示
            switch (winner)
            {
                case State.Winner.P0:
                    p0WinText.gameObject.SetActive(true);
                    break;
                case State.Winner.P1:
                    p1WinText.gameObject.SetActive(true);
                    break;
                case State.Winner.Draw:
                    drawText.gameObject.SetActive(true);
                    break;
            }

            agent0.OnEpisodeEnd(winner);
            agent1.OnEpisodeEnd(winner);
        }
    }

    // プレーヤーに力を加える
    void ActionPlayer(GameObject player, Action a)
    {
        Rigidbody rigidbody = player.GetComponent<Rigidbody>();
        rigidbody.AddForce(ActionToForce(a.forceX), 0f, ActionToForce(a.forceZ));
    }

    float ActionToForce(float a)
    {
        return a * Prefs.forceMag * Random.Range(1f - Prefs.forceNoise, 1f + Prefs.forceNoise);
    }

    // 勝ち負け判定機
    State.Winner Judge(State s)
    {
        bool[] lose = new bool[2];

        // 土俵外に出た判定
        for (int i = 0; i < 2; i++)
        {
            if (s.position[i].x < Prefs.ringWest || Prefs.ringEast < s.position[i].x ||
                s.position[i].z < Prefs.ringSouth || Prefs.ringNorth < s.position[i].z)
            {
                lose[i] = true;
            }
        }

        // 適切な値を返す
        if (lose[0] && lose[1])
        {
            return State.Winner.Draw;
        }
        else if (!lose[0] && lose[1])
        {
            return State.Winner.P0;
        }
        else if (lose[0] && !lose[1])
        {
            return State.Winner.P1;
        }
        return State.Winner.Continue;
    }

    // 新しい Episode を開始する
    void NewGame()
    {
        int sceneIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIdx);
    }

    // EpisodeCount を更新する（表示も）
    void UpdateEpisodeCount()
    {
        episodeCount++;
        episodeCountText.text = "Episode: " + episodeCount.ToString();
    }

    // frameCount を更新する（表示も）
    void UpdateStepCount()
    {
        stepCount++;
        stepCountText.text = "Step: " + stepCount.ToString();
    }

    // スコア表を更新する（表示も）
    // NOTE: これ仕様としてどうなの感ある
    void UpdateScoreText(State.Winner winner)
    {
        switch (winner)
        {
            case State.Winner.P0:
                p0Score++;
                break;
            case State.Winner.P1:
                p1Score++;
                break;
            case State.Winner.Draw:
                drawScore++;
                break;
        }
        p0ScoreText.text = "P0 Win: " + p0Score.ToString();
        p1ScoreText.text = "P1 Win: " + p1Score.ToString();
        drawScoreText.text = "Draw: " + drawScore.ToString();
    }

    // エージェントの愛称を表示するやつ
    void UpdateAgentName()
    {
        agent0Name.text = agent0.AgentName();
        agent1Name.text = agent1.AgentName();
    }

    // 早送りのトグル
    void ToggleFF()
    {
        if (Input.GetKey(Prefs.keyFF))
        {
            isFF = true;
        }
        if (Input.GetKey(Prefs.keyNotFF))
        {
            isFF = false;
        }
        // 速さを変える
        if (isFF)
        {
            Time.timeScale = Prefs.timeScaleFF;
        }
        else
        {
            Time.timeScale = Prefs.timeScaleNormal;
        }
    }

    // State の表示を更新
    void UpdateStateText(State s, Action a0, Action a1)
    {
        stateViewer.text = "";
        stateViewer.text += "Position:\t";
        stateViewer.text += s.position[0].ToString() + "\t\t";
        stateViewer.text += s.position[1].ToString() + "\n";
        stateViewer.text += "Velocity:\t";
        stateViewer.text += s.velocity[0].ToString() + "\t\t";
        stateViewer.text += s.velocity[1].ToString() + "\n";
        stateViewer.text += "Action:\t\t";
        stateViewer.text += a0.ToString() + "\t\t\t\t\t";
        stateViewer.text += a1.ToString();
    }

    // totalScore を更新
    void UpdateTotalScore(State.Winner winner)
    {
        // 対象外のエピソード数のときは飛ばします
        if (Prefs.minScoringEpisode <= episodeCount && episodeCount <= Prefs.maxScoringEpisode)
        {
            switch (winner)
            {
                case State.Winner.P0:
                    totalScore++;
                    break;
                case State.Winner.P1:
                    totalScore--;
                    break;
            }
        }

        totalScoreText.text = "Total Score: " + totalScore.ToString();
    }

    // 初速を与える
    void InitialVelocity()
    {
        Rigidbody r0 = player0.GetComponent<Rigidbody>();
        r0.velocity = new Vector3(Prefs.initialVelocity, 0, 0);
        Rigidbody r1 = player1.GetComponent<Rigidbody>();
        r1.velocity = new Vector3(-Prefs.initialVelocity, 0, 0);
    }

    // ゲームリセットに関する
    void GameReset()
    {
        if (!Input.GetKeyUp(Prefs.keyReset))
        {
            return;
        }
        // static を初期化
        episodeCount = 0;
        p0Score = 0;
        p1Score = 0;
        drawScore = 0;
        totalScore = 0;
        agent0 = null;
        agent1 = null;
        isFF = false;

        // 最初の画面
        SceneManager.LoadScene("Title");
    }
}
