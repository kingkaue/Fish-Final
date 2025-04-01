using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/XP")]
public class XPScript : CollectibleEffect
{
    public int xpAmount;
    public override void Apply(GameObject target)
    {
        GameManager.instance.AddXP(xpAmount);
    }
}
