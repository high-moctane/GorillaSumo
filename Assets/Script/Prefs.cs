using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs
{
    // 土俵の大きさ
    public static readonly float ringWest = -2f;
    public static readonly float ringEast = 2f;
    public static readonly float ringSouth = -2f;
    public static readonly float ringNorth = 2f;

    // 入力できる力の大きさ
    public static readonly float forceMin = -1.0f;
    public static readonly float forceMax = 1.0f;

    // 力の倍率
    public static readonly float forceMag = 7f;

    // 力のノイズ
    public static readonly float forceNoise = 0.05f;

    // 早送りボタン
    public static readonly KeyCode keyFF = KeyCode.V;
    // 通常速ボタン
    public static readonly KeyCode keyNotFF = KeyCode.B;

    // 通常時の timeScale
    public static readonly float timeScaleNormal = 1f;
    // 早送り時の timeScale
    public static readonly float timeScaleFF = 100f;

    // エピソードのラップ回数
    public static readonly int episodeLapCount = 10000;

    // 最大ステップ数
    public static readonly int maxStepCount = 3000;

    // スコアに加算するエピソードの上下
    public static readonly int minScoringEpisode = 5;
    public static readonly int maxScoringEpisode = 10;

    // 初速
    public static readonly float initialVelocity = 2f;

    // 操作するキー
    public static readonly KeyCode keyP0Up = KeyCode.W;
    public static readonly KeyCode keyP0Down = KeyCode.S;
    public static readonly KeyCode keyP0Right = KeyCode.D;
    public static readonly KeyCode keyP0Left = KeyCode.A;
    public static readonly KeyCode keyP1Up = KeyCode.UpArrow;
    public static readonly KeyCode keyP1Down = KeyCode.DownArrow;
    public static readonly KeyCode keyP1Right = KeyCode.RightArrow;
    public static readonly KeyCode keyP1Left = KeyCode.LeftArrow;

    // リセットキー
    public static readonly KeyCode keyReset = KeyCode.R;

    // 結果表示の秒数
    public static readonly float resultDispSec = 1f;
}
