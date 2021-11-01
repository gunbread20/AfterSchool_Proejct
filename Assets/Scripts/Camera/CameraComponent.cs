using UnityEngine;
using UniRx;
using System;

public class CameraComponent : Component
{
    Vector3 offset = new Vector3(10, 10, -10);

    public CameraComponent()
    {
        Observable.EveryUpdate()
            .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
            .Subscribe(Move);
    }

    void Move(long value)
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, offset, Time.deltaTime * 5f);
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<FloorComponent>().Subscribe(CreateFloorEvent);
                break;

            case GameState.STANDBY:
                Reset();
                break;
        }
    }

    void CreateFloorEvent(Floor floor)
    {
        offset.y = 12.5f + floor.transform.position.y;
    }

    void Reset()
    {
        offset = new Vector3(10, 10, -10);

        Camera.main.transform.position = offset;
    }
}
