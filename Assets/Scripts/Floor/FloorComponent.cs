using System.Collections.Generic;
using UnityEngine;

public class FloorComponent : Component
{

    List<Floor> floors = new List<Floor>();

    Vector3 defaultFloorPos = new Vector3(0, -.5f, -12);

    int floorCreateCount = 40;

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<InputComponent>()
                .Subscribe(direction =>
                {
                    if (direction == Direction.Forward)
                    {
                        ReturnSingleFloor();
                        CreateSingleFloor();
                    }

                });
                
                break;
            case GameState.STANDBY:
                CreateFloors();

                break;
            default:
                break;
        }
    }
    void ReturnSingleFloor()
    {
        floors[0].Reset();
        floors.RemoveAt(0);
    }

    void CreateSingleFloor()
    {
        int type = floors.Count % 2;
        //floors[floors.Count - 1].transform.position.z % 2;
        Floor floor = ObjectPool.Instance.GetObject((PoolObjectType)type).GetComponent<Floor>();

        floor.transform.position = GetNextPosition();

        floors.Add(floor);
    }

    void CreateFloors()
    {
        ClearFloors();

        for (int i = 0; i < floorCreateCount; i++)
        {
            int type = floors.Count % 2;

            Floor floor = ObjectPool.Instance.GetObject((PoolObjectType)type).GetComponent<Floor>();

            floor.transform.position = GetNextPosition();

            floors.Add(floor);
        }
    }

    void ClearFloors()
    {
        for (int i = 0; i < floors.Count; i++)
            floors[i].Reset();

        floors.Clear();
     }


    Vector3 GetNextPosition()
    {
        switch (floors.Count)
        {
            case 0:
                return defaultFloorPos;
            default:
                return floors[floors.Count - 1].transform.position + Vector3.forward;
        }
    }
}