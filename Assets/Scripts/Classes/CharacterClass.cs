using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [Header ("Class Stats")]
    public string className;
    public float classBaseMaxHealth;
    public float moveSpeed;
    public float classBaseAttackDamage;

    [Header ("Shooting")]
    public Vector3 aim;
    public bool isPC;
}
