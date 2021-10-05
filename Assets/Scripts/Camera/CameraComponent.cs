using UnityEngine;

public class CameraComponent : Component
{

    private Camera camera;

    private Vector3 distance = new Vector3(3, 10, -5);

    public CameraComponent()
    {
        camera = Camera.main;
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(Follow);
                break;
            case GameState.STANDBY:
                Reset();

                break;
            default:
                break;
        }
    }

    void Follow(Vector3 v3)
    {
        v3.y = 0;
        v3 += distance;

        if (Mathf.Abs(v3.x) > (v3.x > 0 ? 8 : 3))
        v3.x = camera.transform.position.x;

        camera.transform.position = Vector3.Lerp(camera.transform.position, v3, Time.deltaTime * 5f);

        //if (camera.transform.position.x < -3)
        //{
        //    Vector3 cpos = camera.transform.position;
        //    camera.transform.position = new Vector3 (-3f, cpos.y, cpos.z);
        //}
        //else if (camera.transform.position.x > 8)
        //{
        //    Vector3 cpos = camera.transform.position;
        //    camera.transform.position = new Vector3(8f, cpos.y, cpos.z);
        //}
    }

    void Reset()
    {
        camera.transform.position = distance;
    }
}