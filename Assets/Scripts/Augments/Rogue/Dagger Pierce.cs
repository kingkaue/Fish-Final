using UnityEngine;

public class DaggerPierce : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<PlayerManager>().className == "Rogue")
        {
            player.GetComponent<CharacterClass_Rogue>().canPierce = true;
            Debug.Log("Dagger Pierce Activated");
        }
    }
}
