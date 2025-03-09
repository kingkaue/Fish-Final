using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [Header ("Class Stats")]
    public string className;
    public int maxHealth;
    public float moveSpeed;
    public float attackDamage;

    [Header ("Shooting")]
    public Vector3 aim;
    public bool isPC;
}
