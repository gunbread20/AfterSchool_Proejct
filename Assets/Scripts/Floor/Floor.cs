using UnityEngine;

public abstract class Floor : MonoBehaviour
{
    public abstract void Generate();
    public abstract void ReturnTrees();

    public abstract void Reset();
}