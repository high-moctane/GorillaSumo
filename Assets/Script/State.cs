using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    // 勝ちマンの定数
    //   P0 -> Player0 の勝ち
    //   Draw -> 引き分けに終わった
    //   Continue -> ゲームはまだ続いている！（内部で使う）
    public enum Winner { P0, P1, Draw, Continue }

    // プレーヤーの座標
    public List<Vector3> position = new List<Vector3>();
    // プレーヤーの速度
    public List<Vector3> velocity = new List<Vector3>();

    public State(GameObject player0, GameObject player1)
    {
        position.Add(player0.transform.position);
        position.Add(player1.transform.position);
        velocity.Add(player0.GetComponent<Rigidbody>().velocity);
        velocity.Add(player1.GetComponent<Rigidbody>().velocity);
    }
}
