using UnityEngine;
using System.Collections;
using Core.Main;
using Core;
using System.Collections.Generic;
using System;
using Core.Interfaces;

public class PrefabScript : MonoBehaviour
{

    #region Services

    IPrefabService _PrefabService;
    IPrefabService PrefabService
    {
        get
        {
            if (_PrefabService == null)
            {
                _PrefabService = ServiceLocator.GetService<IPrefabService>();
            }
            return _PrefabService;
        }
    }

    #endregion

    #region Properties
    private IDictionary<PrefabNames, Transform> Prefabs;

    #endregion


    // Use this for initialization
    void Start()
    {
        try
        {
            packPrefabs();
            PrefabService.Init(Prefabs);
        }
        catch (Exception e)
        {
            ExceptionHandler.catchException(e);
        }

    }

    void packPrefabs()
    {
        Prefabs = new Dictionary<PrefabNames, Transform>();
        Prefabs.Add(PrefabNames.Vilager, AgentVilager);
        Prefabs.Add(PrefabNames.VilagerRed, AgentVilagerRed);
        Prefabs.Add(PrefabNames.Rock, Rock);
        Prefabs.Add(PrefabNames.RealRock, RealRock);
        Prefabs.Add(PrefabNames.Point, Point);
        Prefabs.Add(PrefabNames.Building, Building);
        Prefabs.Add(PrefabNames.Food, Food);
        Prefabs.Add(PrefabNames.Gold, Gold);
        Prefabs.Add(PrefabNames.Wood, Wood);
        Prefabs.Add(PrefabNames.PointS, PointS);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform AgentVilager;
    public Transform AgentVilagerRed;
    public Transform Rock;
    public Transform RealRock;
    public Transform Point;
    public Transform PointS;
    public Transform Building;
    public Transform Food;
    public Transform Gold;
    public Transform Wood;
}
