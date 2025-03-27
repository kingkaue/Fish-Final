using UnityEngine;

public class SplitShot : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<PlayerManager>().className == "Mage")
        {
            player.GetComponent<CharacterClass_Mage>().splitShotActivated = true;
            Debug.Log("Split Shot Activated");
        }
    }
}
