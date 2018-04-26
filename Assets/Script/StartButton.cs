using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    // Dropdown
    public UnityEngine.UI.Dropdown p0Agent, p1Agent;

    // スタートボタンが押されたときの処理
    public void OnStartButtonUp()
    {
        // Player0 のエージェントをセット
        switch (p0Agent.value)
        {
            case 0:
                GameController.agent0 = new AgentB4(Agent.ID.P0);
                break;
            case 1:
                GameController.agent0 = new AgentM1(Agent.ID.P0);
                break;
            case 2:
                GameController.agent0 = new AgentM2(Agent.ID.P0);
                break;
            case 3:
                GameController.agent0 = new SampleAgent(Agent.ID.P0);
                break;
            case 4:
                GameController.agent0 = new AgentPID(Agent.ID.P0);
                break;
            case 5:
                GameController.agent0 = new AgentManual(Agent.ID.P0);
                break;
        }
        // Player0 のエージェントをセット
        switch (p1Agent.value)
        {
            case 0:
                GameController.agent1 = new AgentB4(Agent.ID.P1);
                break;
            case 1:
                GameController.agent1 = new AgentM1(Agent.ID.P1);
                break;
            case 2:
                GameController.agent1 = new AgentM2(Agent.ID.P1);
                break;
            case 3:
                GameController.agent1 = new SampleAgent(Agent.ID.P1);
                break;
            case 4:
                GameController.agent1 = new AgentPID(Agent.ID.P1);
                break;
            case 5:
                GameController.agent1 = new AgentManual(Agent.ID.P1);
                break;
        }

        Time.timeScale = Prefs.timeScaleNormal;

        GameController.agent0.OnGameStart();
        GameController.agent1.OnGameStart();

        SceneManager.LoadScene("SumoRing");
    }
}
