using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentM2 : Agent
{
    int myID;
    public AgentM2(Agent.ID ID) : base(ID) {
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
        float myX = s.position[myID].x;
        float myZ = s.position[myID].z;
        float targetX = s.position[1-myID].x;
        float targetZ = s.position[1-myID].z;

        return new Action(targetX - myX, targetZ - myZ);
    }
}
