using UnityEngine;

public class CameraComponent : Component
{

    private Camera camera;

    private Vector3 distance = new Vector3(3, 10, -5);

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                camera = Camera.main;

                GameManager.Instance.GetGameBaseComponent<PlayerComponent>()
                    .Subscribe(Follow);
                break;
            case GameState.STANDBY:
                Reset();

                break;
        }
    }

    void Follow(Vector3 v3)
    {
        v3.y = 0;
        v3 += distance;

        camera.transform.position = Vector3.Lerp(camera.transform.position, v3, Time.deltaTime * 5f);
    }

    void Reset()
    {
        camera.transform.position = distance;
    }
}

