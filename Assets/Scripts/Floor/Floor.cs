using UnityEngine;
using DG.Tweening;

public class Floor : MonoBehaviour
{

    public void Move(int lotation)
    { // 0 = x L to R, 1 = y U to D, 2 = x R to L, 3 = y D to U

        //transform.DOMoveX(5, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        switch (lotation)
        {
            case 0:
                transform.DOMoveX(5, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                return;

            //case 1:
            //    transform.DOMoveZ(5, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            //    return;

            case 1:
                transform.DOMoveX(-5, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                return;

            //case 3:
            //    transform.DOMoveZ(-5, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            //    return;

            default:
                return;
        }


    }

    public void Stop()
    {
        transform.DOKill();

        if (Mathf.Abs(transform.position.x) >= 3 || Mathf.Abs(transform.position.z) >= 3)
        {
            GameManager.Instance.UpdateState(GameState.OVER);
        }
    }

    public void Reset()
    {
        transform.DOKill();
        ObjectPool.Instance.ReturnObject(PoolObjectType.Floor, gameObject);
    }

}
