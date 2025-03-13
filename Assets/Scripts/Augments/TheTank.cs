using UnityEngine;

public class TheTank : MonoBehaviour
{
    // When augment game object is activated, doubles player's damage and multiplies health by 0.8
    void OnEnable()
    {
        // Damage handling
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetDamageMultiplier(0.7f);
        float damageMult = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().damageMultiplier;
        float baseDmg = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().baseAttackDamage;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetAttackDamage(damageMult, baseDmg);

        // Health handling
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().MultiplyMaxHealth(2f);
    }
}
