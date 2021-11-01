using UnityEngine;
using UniRx;
using System;

public class InputComponent : Component
{

    protected IObservable<long> inputStream;

    public InputComponent()
    {
        inputStream = Observable.EveryUpdate()
             .Where(_ => Input.GetMouseButtonDown(0))
             .Where(_ => GameManager.Instance.STATE ==
             GameState.RUNNING);
    }

    public void UpdateState(GameState state)
    {
     
    }

    public void Subscribe(Action<long> action)
    {
        inputStream.Subscribe(action);
    }
}