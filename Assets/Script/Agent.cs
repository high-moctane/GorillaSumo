using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent
{
    public enum ID { P0, P1 };

    // playerID には 0 か 1 が与えられる
    public Agent(Agent.ID playerID) { }

    // ゲーム開始時に呼ばれる
    public abstract void OnGameStart();

    // ゲーム終了時に呼ばれる
    public abstract void OnGameEnd();

    // 開始時に呼ばれる
    public abstract void OnEpisodeStart();

    // 終了時に呼ばれる
    public abstract void OnEpisodeEnd(State.Winner winner);

    // エージェントの愛称
    public abstract string AgentName();

    // 1ステップ
    public abstract Action Step(State s);
}
