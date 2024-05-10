using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameEvent
{


    //Game Management
    OnGameStart,
    OnIncreaseScore,
    OnUIUpdate,
    OnUIGameOver,
    OnNextLevel,
    OnGameOver
}
public class EventManager
{
    private static Dictionary<GameEvent,Action> eventTable = new Dictionary<GameEvent, Action>();
    
    private static Dictionary<GameEvent,Action<int>> IdEventTable=new Dictionary<GameEvent, Action<int>>();
    //Two Parameters, key and listener

    
    public static void AddHandler(GameEvent gameEvent,Action action)
    {
        if(!eventTable.ContainsKey(gameEvent))
            eventTable[gameEvent]=action;
        else eventTable[gameEvent]+=action;
    }

    public static void RemoveHandler(GameEvent gameEvent,Action action)
    {
        if(eventTable[gameEvent]!=null)
            eventTable[gameEvent]-=action;
        if(eventTable[gameEvent]==null)
            eventTable.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent gameEvent)
    {
        if(eventTable[gameEvent]!=null)
            eventTable[gameEvent]();
    }

    //ID
    public static void AddIdHandler(GameEvent gameIdEvent,Action<int> actionId)
    {
        if(!IdEventTable.ContainsKey(gameIdEvent))
            IdEventTable[gameIdEvent]=actionId;
        else IdEventTable[gameIdEvent]+=actionId;
    }

    public static void RemoveIdHandler(GameEvent gameIdEvent,Action<int> actionId)
    {
        if(IdEventTable[gameIdEvent]!=null)
            IdEventTable[gameIdEvent]-=actionId;
        if(IdEventTable[gameIdEvent]==null)
            IdEventTable.Remove(gameIdEvent);
    }

    public static void BroadcastId(GameEvent gameIdEvent,int id)
    {
        if(IdEventTable[gameIdEvent]!=null)
            IdEventTable[gameIdEvent](id);
    }

    
}
