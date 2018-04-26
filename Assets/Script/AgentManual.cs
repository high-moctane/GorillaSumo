using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManual : Agent
{
    KeyCode up, down, right, left;

    public AgentManual(Agent.ID ID) : base(ID)
    {
        if (ID == Agent.ID.P0)
        {
            up = Prefs.keyP0Up;
            down = Prefs.keyP0Down;
            right = Prefs.keyP0Right;
            left = Prefs.keyP0Left;
        }
        else
        {
            up = Prefs.keyP1Up;
            down = Prefs.keyP1Down;
            right = Prefs.keyP1Right;
            left = Prefs.keyP1Left;
        }
    }

    public override Action Step(State s)
    {
        float x = 0f, z = 0f;
        if (Input.GetKey(up))
        {
            z += 1f;
        }
        if (Input.GetKey(down))
        {
            z += -1f;
        }
        if (Input.GetKey(right))
        {
            x += 1f;
        }
        if (Input.GetKey(left))
        {
            x += -1f;
        }
        return new Action(x, z);
    }

    public override string AgentName()
    {
        return "Manual";
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
}
