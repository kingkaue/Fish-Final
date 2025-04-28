using UnityEngine;

public class CritStrike : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<PlayerManager>().className == "Fighter")
        {
            player.GetComponent<CharacterClass_FIghter>().canCrit = true;
            Debug.Log("Crit Strike Activated");
        }
    }
}
