using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public string name;
    public Sprite icon;
    public float damage;
}

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Sprite defaultWeaponIcon;
    public Sprite CurrentWeaponIcon { get; private set; }
    public event Action<Sprite> OnWeaponChanged;

    void Start()
    {
        EquipDefault();
    }

    public void EquipDefault()
    {
        CurrentWeaponIcon = defaultWeaponIcon;
        OnWeaponChanged?.Invoke(CurrentWeaponIcon);
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        CurrentWeaponIcon = newWeapon.icon;
        OnWeaponChanged?.Invoke(CurrentWeaponIcon);
    }
}