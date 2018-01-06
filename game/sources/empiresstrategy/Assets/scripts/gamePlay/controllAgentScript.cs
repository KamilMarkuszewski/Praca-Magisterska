using UnityEngine;
using System.Collections;
using Core.PlayerData;
using Core.Main;
using Core.Interfaces;

public class controllAgentScript : MonoBehaviour, IcontrollAgentScript
{

    #region Services

    private static AgentService _agentService;
    public static AgentService agentService
    {
        get
        {
            if (_agentService == null) _agentService = ServiceLocator.GetService<AgentService>();
            return _agentService;
        }
        set
        {
            _agentService = value;
        }
    }

    #endregion


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            agentService.unselectAll();
        }
        agentService.update();
        agentService.agentUpd(this.gameObject, agentData);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            agentService.agentClicked(this.gameObject, agentData);
        }
    }

    private object _agentData;
    public object agentData
    {
        get
        {
            return _agentData;
        }
        set
        {
            _agentData = value;
        }
    }
}
