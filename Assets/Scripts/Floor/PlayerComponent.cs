using DG.Tweening;
using System;
using UniRx;
using UnityEngine;

public class PlayerComponent : Component
{

    GameObject player;

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                player = ObjectPool.Instance.GetObject(PoolObjectType.Player);
                
                GameManager.Instance.GetGameBaseComponent<InputComponent>().Subscribe(Move);

                break;
            case GameState.STANDBY:
                player.transform.position = Vector3.zero;
                break;
            default:
                break;
        }
    }

    void Move(Direction direction)
    {
        Vector3 pos = player.transform.position + GetConvertedDirectionV3(direction);

        player.transform.DOScaleY(.7f, .08f).OnComplete(() =>
        {
            player.transform.DOLocalJump(pos, 1, 1, .32f);
            player.transform.DOScaleY(1, .08f);
            player.transform.DOLocalRotate(GetConvertedRotateV3(direction),.32f,RotateMode.Fast);
        });
    }

    Vector3 GetConvertedDirectionV3(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.forward;
            case Direction.Back:
                return Vector3.back;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    Vector3 GetConvertedRotateV3(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.zero;
            case Direction.Back:
                return Vector3.down * 180;
            case Direction.Left:
                return Vector3.down * 90;
            case Direction.Right:
                return Vector3.up * 90;
            default:
                return Vector3.zero;
        }
    }

    private IObservable<Vector3> moveStream;

    public PlayerComponent()
    {
        moveStream = Observable.EveryUpdate()
        .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
        .Select(_ => player.transform.position)
        .DistinctUntilChanged();
    }

    public void Subscribe(Action<Vector3> action)
    {
        moveStream.Subscribe(action);
    }
}