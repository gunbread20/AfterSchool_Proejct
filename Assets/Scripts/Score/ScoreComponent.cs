using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ScoreComponent : Component
{
    int score = 0;
    int condtion = 0;

    IObservable<int> scoreStream;

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            // in UpdateState Function.
            case GameState.INIT:
                scoreStream = Observable.EveryUpdate()
                    .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
                    .Select(_ => score)
                    .DistinctUntilChanged();

                GameManager.Instance.GetGameBaseComponent<InputComponent>().Subscribe(UpdateScore);

                break;

            case GameState.STANDBY:
                score = 0;
                condtion = 0;
                break;
        }
    }

    void UpdateScore(Vector3 pos)
    {
        if (condtion < pos.z)
        {
            score++;

            condtion++;

            Debug.Log(score);
        }
    }

    public void Subscribe(Action<int> action)
    {
        scoreStream.Subscribe(action);
    }

}