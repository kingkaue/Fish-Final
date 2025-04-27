using UnityEngine;

public class Cheats : MonoBehaviour
{
    public int damageAmount = 10;
    public int healAmountPercent = 10;  // as percent of max
    public int xpAmount = 50;

    private PlayerManager pm;
    private GameManager gm;

    void Awake()
    {
        pm = FindObjectOfType<PlayerManager>();
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))  // H for hit
        {
            pm.TakeDamage(damageAmount);
            Debug.Log($"Cheat: Took {damageAmount} damage");
        }
        if (Input.GetKeyDown(KeyCode.J))  // J for heal
        {
            pm.Heal(healAmountPercent / 100f);
            Debug.Log($"Cheat: Healed {healAmountPercent}%");
        }
        if (Input.GetKeyDown(KeyCode.K))  // K for xp
        {
            gm.AddXP(xpAmount);
            Debug.Log($"Cheat: Gained {xpAmount} XP");
        }
    }
}