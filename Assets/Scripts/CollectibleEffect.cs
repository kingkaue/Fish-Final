using UnityEngine;

public abstract class CollectibleEffect : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
