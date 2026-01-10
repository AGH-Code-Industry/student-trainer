using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.Linq;

public class ArenaManager : MonoBehaviour
{
    private static ArenaManager _instance;

    public static ArenaManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    [SerializeField] List<EnemyAgent> agents = new List<EnemyAgent>();

    public bool EpisodeEnded { get; private set;}
    bool isResetting = false;

    void Start()
    {
        if (agents.Count == 0)
        {
            agents.AddRange(FindObjectsByType<EnemyAgent>(FindObjectsSortMode.None));
        }
    }

    public void OnAgentDied(EnemyAgent deadAgent)
    {
        if (EpisodeEnded)
            return;

        int aliveCount = 0;
        EnemyAgent lastAlive = null;

        foreach (var agent in agents)
        {
            if (agent.IsAlive)
            {
                aliveCount++;
                lastAlive = agent;
            }
        }

        if (aliveCount <= 1)
        {
            EndEpisodeForAll(lastAlive);
        }
    }

    void EndEpisodeForAll(EnemyAgent winner)
    {
        EpisodeEnded = true;

        foreach (var agent in agents)
        {
            if (agent == winner)
            {
                agent.AddReward(+10f); // zwycięzca
            }

            agent.transform.gameObject.SetActive(true);

            agent.EndEpisode();
        }

        EpisodeEnded = false;
    }

    public void RequestResetAllAgents()
    {
        foreach (var a in agents)
        {
            if(!a.gameObject.activeInHierarchy)
            {
                a.transform.gameObject.SetActive(true);
                a.OnEpisodeBegin();
            }
        }
    }

}
