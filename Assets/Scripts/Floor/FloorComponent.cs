using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class FloorComponent : Component
{

    IObservable<Floor> floorCreateStream;

    GameObject baseFloor;
    List<Floor> floors = new List<Floor>();
    int lotation = 0;

    public FloorComponent()
    {
        floorCreateStream = Observable.EveryUpdate()
            .Where(_ => GameManager.Instance.STATE == GameState.RUNNING && floors.Count > 0)
            .Select(_ => floors.Count)
            .DistinctUntilChanged()
            .Select(_ => floors[floors.Count - 1]);


    }

    public void Subscribe(Action<Floor> action)
    {
        floorCreateStream.Subscribe(action);
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                Init();

                GameManager.Instance.GetGameBaseComponent<InputComponent>().
                    Subscribe(CreateFloor);
                break;

            case GameState.STANDBY:
                Reset();
                break;

            case GameState.RUNNING:
                CreateFloor(0);
                break;
        }
    }

    void CreateFloor(long value)
    {
        if (floors.Count > 0)
        {
            floors[floors.Count - 1].Stop();
            Slice(floors[floors.Count - 1]);
        }
            

        GameObject floor = ObjectPool.Instance.GetObject(PoolObjectType.Floor);

        floors.Add(floor.GetComponent<Floor>());

        floors[floors.Count - 1].Move(floors.Count % 2);

        floor.transform.position = GetNextFloorPosition();
    }

    Vector3 GetNextFloorPosition()
    {
        float nextYPos = 0;
        float nextXPos = 0;
        float nextZPos = 0;

        switch (floors.Count % 2)
        {
            case 0: nextXPos = -5; break;
            //case 1: nextZPos = -5; break;
            case 1: nextXPos = 5; break;
            //case 3: nextZPos = 5; break;
            default: break;
        }

        if (floors.Count > 1)
        {
            nextYPos =  (floors[floors.Count - 2].transform.localScale.y * .5f) +
                        floors[floors.Count - 2].transform.position.y + 
                        (floors[floors.Count - 1].transform.localScale.y * .5f);
        }

        return new Vector3(nextXPos, nextYPos, nextZPos);
    }

    void Init()
    {
        baseFloor = ObjectPool.Instance.
            GetObject(PoolObjectType.BaseFloor);

        baseFloor.transform.position
            = new Vector3(0, -5.5f, 0);
    }

    void Reset()
    {
        for (int i = 0; i < floors.Count; i++)
            floors[i].Reset();

        floors.Clear();
    }

    void Slice(Floor floor)
    {
        float distance = floor.transform.position.x - baseFloor.transform.position.x;

        float newXSize = baseFloor.transform.localScale.x - Math.Abs(distance);
        float newXPosition = baseFloor.transform.position.x + (distance * .5f);

        floor.transform.localScale = new Vector3(newXSize, floor.transform.localScale.y, floor.transform.localScale.z);
        floor.transform.position = new Vector3(newXPosition, floor.transform.position.y, floor.transform.position.z);
    }
}