using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ScoreComponent : Component
{
    int score = 0;
    int bestscore = 0;
    int condtion = 0;

    IObservable<ScoreData> scoreStream;

    readonly string bsetScoreKey = "bestScoreKey";

    public ScoreComponent()
    {
        if (!PlayerPrefs.HasKey(bsetScoreKey))
            PlayerPrefs.SetInt(bsetScoreKey, 0);
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.STANDBY:
                score = 0;
                condtion = 0;
                break;

            case GameState.INIT:

                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(UpdateScore);
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(CheckLog);
                GameManager.Instance.GetGameBaseComponent<InputComponent>().Subscribe(BackMinus);
                

                scoreStream = Observable.EveryUpdate()
                    .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
                    .Select(_ => score)
                    .DistinctUntilChanged()
                    .Select(_ => new ScoreData(score, bestscore));
                break;

            default:
                break;
        }
    }

    public void Subscribe(Action<ScoreData> action)
    {
        scoreStream.Subscribe(action);
    }


    void BackMinus(Direction direction)
    {
        switch (direction)
        {
            case Direction.Back:
                score--;
                break;
        }
    }

    void CheckLog(Collider parent)
    {
        if (parent.tag == "Log")
        {
            score += 5;
        }
    }

    void UpdateScore(Vector3 pos)
    {
        if (condtion < pos.z)
        {
            score++;

            condtion++;

            if (condtion % 10 == 0)
            {
                score += condtion;
            }

            if (bestscore < score)
            {
                PlayerPrefs.SetInt(bsetScoreKey, score);

                bestscore = score;
            }
        }
    }
}
