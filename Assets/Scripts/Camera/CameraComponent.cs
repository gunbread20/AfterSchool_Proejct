using DG.Tweening;
using System.Threading;
using UnityEngine;

public class CameraComponent : Component
{

    private Camera camera;
    private Vector3 firstPos;
    private Vector3 distance = new Vector3(3, 10, -5);

    public CameraComponent()
    {
        camera = Camera.main;
        firstPos = camera.transform.position;
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(Follow);
                break;
            case GameState.STANDBY:
                camera.transform.position = firstPos;
                break;
            default:
                break;
        }
    }

    void Follow(Vector3 v3)
    {
        v3.y = firstPos.y - 10;
        camera.transform.position = v3 + distance; ;
    }
}