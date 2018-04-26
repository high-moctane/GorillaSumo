using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SampleAgent : Agent
{

    // 識別子
    Agent.ID myID;
    int IDnum;

    float[][] QTable;

    float alpha = 0.1f;
    float gamma = 0.99f;

    float eps = 0.1f;

    MyAction myAction, lastMyAction;
    MyState myState, lastMyState;

    public SampleAgent(Agent.ID id) : base(id)
    {
        myID = id;
        IDnum = (int)myID;
    }

    public override void OnEpisodeStart()
    {
    }

    public override void OnEpisodeEnd(State.Winner winner)
    {
        QTable[lastMyState.Idx()][lastMyAction.Idx()] +=
            alpha * (LastReward(winner) + gamma * QTable[myState.Idx()][myAction.Idx()]
             - QTable[lastMyState.Idx()][lastMyAction.Idx()]);
    }

    public override string AgentName()
    {
        return "(｀･ω･´)";
    }

    public override void OnGameStart()
    {
        InitQTable();
    }

    public override void OnGameEnd()
    {
    }

    void InitQTable()
    {
        QTable = new float[MyState.posNum * MyState.posNum * MyState.posNum * MyState.posNum * MyState.velNum * MyState.velNum * MyState.velNum * MyState.velNum][];
        for (int i = 0; i < QTable.Length; i++)
        {
            QTable[i] = new float[MyAction.stepNum * MyAction.stepNum];

            for (int j = 0; j < QTable[i].Length; j++)
            {
                QTable[i][j] = UnityEngine.Random.Range(80f, 80.1f);
            }
        }
    }

    int ArgMax(float[] array)
    {
        float max = -Mathf.Infinity;
        int idx = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (max < array[i])
            {
                max = array[i];
                idx = i;
            }
        }
        return idx;
    }

    float Max(float[] array)
    {
        float max = -Mathf.Infinity;
        for (int i = 0; i < array.Length; i++)
        {
            if (max < array[i])
            {
                max = array[i];
            }
        }
        return max;
    }

    public override Action Step(State s)
    {
        myState = new MyState(s, IDnum);

        if (lastMyState != null && lastMyState.Idx() == myState.Idx())
        {
            return lastMyAction.Action();
        }

        int actIdx = ArgMax(QTable[myState.Idx()]);
        myAction = new MyAction(actIdx);
        if (UnityEngine.Random.Range(0f, 1f) < eps)
        {
            new MyAction((int)UnityEngine.Random.Range(0f, MyAction.stepNum * MyAction.stepNum - 1));
        }

        // Learn
        if (lastMyState != null)
        {
            QTable[lastMyState.Idx()][lastMyAction.Idx()] +=
                alpha * (Reward(s) + gamma * QTable[myState.Idx()][myAction.Idx()]
                 - QTable[lastMyState.Idx()][lastMyAction.Idx()]);
        }

        lastMyAction = myAction;
        lastMyState = myState;

        return myAction.Action();
    }

    float Reward(State s)
    {
        return Mathf.Abs(s.position[1 - IDnum].x) + Mathf.Abs(s.position[1 - IDnum].z);
        // return Mathf.Abs(s.position[1 - IDnum].x) - Mathf.Abs(s.position[IDnum].x) +
        //     Mathf.Abs(s.position[1 - IDnum].z) - Mathf.Abs(s.position[IDnum].z);
    }

    float LastReward(State.Winner winner)
    {
        if (winner == State.Winner.Draw)
        {
            return -10f;
        }
        if (IDnum == (int)winner)
        {
            return 50f;
        }
        return -50f;
    }

    class MyState
    {
        // しきい値
        public static float[] posThresh = new float[] { 0f };
        public static float[] velThresh = new float[] { 0f };

        // 分割数
        public static int posNum = posThresh.Length + 1, velNum = velThresh.Length + 1;

        // 内部の State
        State state;
        int id;

        public MyState(State s, int idnum)
        {
            state = s;
            id = idnum;
        }

        // 何番目か出す
        int MyIdx(float val, float[] thresh)
        {
            int i = 0;
            for (; i < thresh.Length; i++)
            {
                if (val < thresh[i])
                {
                    return i;
                }
            }
            return i;
        }

        // State の番号
        public int Idx()
        {
            return MyIdx(state.position[id].x, posThresh) +
                MyIdx(state.position[id].z, posThresh) * posNum +
                (state.position[1 - id].x - state.position[id].x > 0 ? 1 : 0) * posNum * posNum +
                (state.position[1 - id].z - state.position[id].z > 0 ? 1 : 0) * posNum * posNum * posNum +
            // return MyIdx(state.position[0].x, posThresh) +
            //     MyIdx(state.position[0].z, posThresh) * posNum +
            //     MyIdx(state.position[1].x, posThresh) * posNum * posNum +
            //     MyIdx(state.position[1].z, posThresh) * posNum * posNum * posNum +
                MyIdx(state.velocity[0].x, velThresh) * posNum * posNum * posNum * posNum +
                MyIdx(state.velocity[0].z, velThresh) * posNum * posNum * posNum * posNum * velNum +
                MyIdx(state.velocity[1].x, velThresh) * posNum * posNum * posNum * posNum * velNum * velNum +
                MyIdx(state.velocity[1].z, velThresh) * posNum * posNum * posNum * posNum * velNum * velNum * velNum;
        }
    }

    class MyAction
    {
        // とりうる値
        public static float[] steps = new float[] { -1f, 0f, 1f };
        public static int stepNum = steps.Length;

        int index;

        public MyAction(int idx)
        {
            index = idx;
        }

        public MyAction(Action a)
        {
            index = XZToIdx(a.forceX, a.forceZ);
        }

        public Action Action()
        {
            return new Action(IdxToX(index), IdxToZ(index));
        }

        public int Idx()
        {
            return index;
        }

        float IdxToX(int idx)
        {
            return steps[idx % stepNum];
        }

        float IdxToZ(int idx)
        {
            return steps[idx / stepNum];
        }

        int XZToIdx(float x, float z)
        {
            return Array.IndexOf(steps, x) + Array.IndexOf(steps, z) * stepNum;
        }
    }
}
