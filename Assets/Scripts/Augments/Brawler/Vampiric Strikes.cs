using UnityEngine;

public class VampiricStrikes : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<PlayerManager>().className == "Fighter")
        {
            player.GetComponent<CharacterClass_FIghter>().isVampiricStrike = true;
            Debug.Log("Vampiric Strike Activated");
        }
    }
}
