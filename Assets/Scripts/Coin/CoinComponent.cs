using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class CoinComponent : Component
{
    readonly string coinKey = "coinKey";

    int coin = 0;

    IObservable<int> coinStream;

    public CoinComponent()
    {
        if (!PlayerPrefs.HasKey(coinKey))
            PlayerPrefs.SetInt(coinKey, 0);
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<FloorComponent>().Subscribe(FloorEvent);
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(UpdateCoin);

                coinStream = Observable.EveryUpdate()
                    .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
                    .Select(_ => coin)
                    .DistinctUntilChanged();


                break;
            
            default:
                break;
        }
    }

    void FloorEvent(Floor floor)
    {
        if (UnityEngine.Random.Range(0, 100) > 50)
        {
            floor.CreateCoin();
        }
    }

    public void Subscribe(Action<int> action)
    {
        coinStream.Subscribe(action);
    }

    void UpdateCoin(Collider collider)
    {
        switch (collider.tag)
        {
            case "Coin":
                PlayerPrefs.SetInt(coinKey, PlayerPrefs.GetInt(coinKey) + 5);

                coin = PlayerPrefs.GetInt(coinKey);

                ObjectPool.Instance.ReturnObject(PoolObjectType.Coin, collider.transform.parent.gameObject);
                break;
        }
    }
}