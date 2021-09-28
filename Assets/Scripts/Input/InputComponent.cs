using UniRx;
using System;
using UnityEngine;

public abstract class InputComponent : Component
{

    protected IObservable<Direction> inputStream;

    public InputComponent()
    {
        inputStream = Observable.EveryUpdate()
        .Where(_ => Input.anyKeyDown)
        .ThrottleFirst(TimeSpan.FromMilliseconds(250))
        .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
        .Select(_ => GetDirection(Input.inputString));
    }

    public void UpdateState(GameState state) { }

    public abstract Direction GetDirection(string keycode);

    public void Subscribe(Action<Direction> action)
    {
        inputStream.Subscribe(action);
    }

}