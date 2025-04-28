using UnityEngine;

public class BombDash : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<PlayerManager>().className == "Rogue")
        {
            player.GetComponent<CharacterClass_Rogue>().bombDrop = true;
            Debug.Log("Bomb Drop Activated");
        }
    }
}
