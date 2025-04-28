using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BubbleBurn : MonoBehaviour
{
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<PlayerManager>().className == "Mage")
        {
            player.GetComponent<CharacterClass_Mage>().bubbleBurnActivated = true;
            Debug.Log("Bubble Burn Activated");
        }
    }
}
