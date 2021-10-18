using UnityEngine;
using DG.Tweening;

public class EagleComponent : Component
{

    GameObject engle;

    public EagleComponent()
    {

    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(Kill);

                break;
            default:
                break;
        }
    }

    public void Kill(GameObject player)
    {
        Debug.Log("Kill");

        engle = ObjectPool.Instance.GetObject(PoolObjectType.Eagle);

        GameManager.Instance.UpdateState(GameState.OVER);

        engle.transform.position = player.transform.position + (Vector3.up * 2) + (Vector3.right * 25);

        engle.transform.DOMoveX(player.transform.position.x, .75f).SetEase(Ease.Linear).OnComplete(() =>
        {
            player.transform.SetParent(engle.transform, true);

            engle.transform.DOMoveX(player.transform.position.x - 25, .75f).SetEase(Ease.Linear).OnComplete(() =>
            {
                ObjectPool.Instance.ReturnObject(PoolObjectType.Eagle, engle);
            });
        });
    }
}