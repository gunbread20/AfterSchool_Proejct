using DG.Tweening;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerComponent : Component
{

    GameObject player;
    Collider carHit;

    IObservable<Vector3> moveStream;

    public PlayerComponent()
    {
        moveStream = Observable.EveryUpdate()
            .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
            .Select(_ => player.transform.position);
    }

    public void Subscribe(Action<Vector3> action)
    {
        moveStream.Subscribe(action);
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                player = ObjectPool.Instance.GetObject(PoolObjectType.Player);

                GameManager.Instance.GetGameBaseComponent<InputComponent>().Subscribe(Move);

                player.OnTriggerEnterAsObservable().Subscribe(collider =>
                {
                    switch (collider.tag)
                    {
                        case "Car":
                            if (GameManager.Instance.STATE != GameState.RUNNING)
                            {                               
                                return;
                            }
                               
                            GameManager.Instance.UpdateState(GameState.OVER);
                            player.transform.DOScaleY(.1f, .16f);
                            break;
                        default:
                            break;
                    }
                });

                break;
            case GameState.STANDBY:
                Reset();
                break;

            //case GameState.OVER:
                
            //    Reset();
            //    break;

            default:
                break;
        }
    }

    void Reset()
    {
        player.transform.SetParent(null);

        player.transform.localScale = Vector3.one;
        player.transform.position = Vector3.zero;
        player.transform.localEulerAngles = Vector3.zero;
    }

    void Move(Direction direction)
    {
        if (direction == Direction.None) return;

        player.transform.DOScaleY(.8f, .08f).OnComplete(() =>
        {
            player.transform.DOScaleY(1, .08f);

            Vector3 pos = player.transform.position;

            RaycastHit hit;

            //if (Physics.Raycast(player.transform.position + Vector3.up, GetConvertedDirectionV3(direction), out hit, 1f))
            //    Debug.Log(hit.collider.name);
            //else
            //    pos += GetConvertedDirectionV3(direction);

            if (Physics.Raycast(player.transform.position + Vector3.up, GetConvertedDirectionV3(direction), out hit, 1f))
            {
                switch (hit.collider.tag)
                {
                    case "Car":
                        Vector3 movePos = hit.normal; // 충돌 법선
                        movePos.z *= .5f; // 자동차 앞뒤 포지션 수정

                        Vector3 scale = Vector3.one; // 플레이어 크기 초기값
                        scale.z *= .25f; // 찌부러진 플레이어 처럼 보이도록 크기 조절

                        player.transform.DOKill(); // 플레이어 Dotween 움직임 모두 중단.

                        player.transform.SetParent(hit.collider.transform, true); // 플레이어 자동차에 하위 객체로 가도록 수정

                        player.transform.DOScale(scale, .16f); // 크기 수정

                        player.transform.DOLocalMove(movePos, .16f); // 플레이어 움직임 시작

                        player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);

                        GameManager.Instance.UpdateState(GameState.OVER);
                        return;
                    default:
                        break;
                }
            }
            else
            {
                pos += GetConvertedDirectionV3(direction);
            }

            player.transform.DOJump(pos, 1, 1, .32f).OnComplete(() =>
             {
                 player.transform.position = new Vector3((float)Math.Round(player.transform.position.x, 1), 0, (float)Math.Round(player.transform.position.z, 1));
             });

            player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);
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
}