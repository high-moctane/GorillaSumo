using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPID : Agent
{
    // ID
    int IDnum;

    // ゲイン
    float kp = 0.5f;
    float ki = 0.002f;
    float kd = 0.15f;

    // 積分値
    float sumX, sumZ;

    public AgentPID(Agent.ID ID) : base(ID)
    {
        IDnum = (int)ID;
    }

    // ゲーム開始時に呼ばれる
    public override void OnGameStart()
    {
    }

    // ゲーム終了時に呼ばれる
    public override void OnGameEnd()
    {
    }

    // 開始時に呼ばれる
    public override void OnEpisodeStart()
    {
        sumX = 0f;
        sumZ = 0f;
    }

    // 終了時に呼ばれる
    public override void OnEpisodeEnd(State.Winner winner)
    {
    }

    // エージェントの愛称
    public override string AgentName()
    {
        return "Agent PID";
    }

    // 1ステップ
    public override Action Step(State s)
    {
        Vector3 pos = s.position[IDnum];
        Vector3 vel = s.velocity[IDnum];
        sumX += pos.x;
        sumZ += pos.z;
        return new Action(Gain(pos.x, vel.x), Gain(pos.z, vel.z));
    }

    // ゲインを決めます
    float Gain(float pos, float vel)
    {
        return -pos * kp + -sumX * ki + -vel * kd;
    }

}
