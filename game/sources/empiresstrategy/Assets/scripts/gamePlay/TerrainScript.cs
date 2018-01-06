using UnityEngine;
using System.Collections;
using Core.PlayerData;
using Core.Main;
using Core.Interfaces;

public class TerrainScript : MonoBehaviour
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

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float f = 200.0f;
            if (Physics.Raycast(ray, out hit, f))
            {
                Vector2 hitPoint = new Vector2(hit.point.x, hit.point.z);
                agentService.setDestination(hitPoint);
            }

        }
    }
}
