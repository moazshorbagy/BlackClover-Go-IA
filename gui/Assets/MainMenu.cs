using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Board
{
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        AgentManager.agentvsagent=true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void TrainerMode()
    {
        AgentManager.agentvsagent=false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
}