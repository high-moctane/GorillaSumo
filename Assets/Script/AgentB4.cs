using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentB4 : Agent
{
    int myID;

    float k = 1.6f;
    float kp = 0.5f;
    float kd = 0.3f;

    public AgentB4(Agent.ID ID) : base(ID) {
        myID = (int)ID;
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
    }

    // 終了時に呼ばれる
    public override void OnEpisodeEnd(State.Winner winner)
    {
    }

    // エージェントの愛称
    public override string AgentName()
    {
        return "ore ore agent";
    }

    // 1ステップ
    public override Action Step(State s)
    {
        if (
            (Mathf.Abs(s.position[myID].x) > 1.5f &&
            Mathf.Abs(s.position[myID].z) > 1.5f) &&
            (Mathf.Abs(s.velocity[myID].x) > 1.5f ||
            Mathf.Abs(s.velocity[myID].z) > 1.5f)
        ) {
            return PID(s);
        }
        return Chase(s);
    }

    Action Chase(State s) {
        float myX = s.position[myID].x;
        float myZ = s.position[myID].z;

        float r2 =
            Mathf.Pow(s.position[1-myID].x, 2) +
            Mathf.Pow(s.position[1-myID].z, 2);
        float r = Mathf.Sqrt(r2);

        float targetX = s.position[1-myID].x * (r - 0.5f);
        float targetZ = s.position[1-myID].z * (r- 0.5f);

        return new Action(k * (targetX - myX), k * (targetZ - myZ));
    }

    Action PID(State s) {
        return new Action(
            Gain(s.position[myID].x, s.velocity[myID].x),
            Gain(s.position[myID].z, s.velocity[myID].z)
        );
    }

    float Gain(float pos, float vel) {
        return -pos * kp + -vel * kd;
    }
}