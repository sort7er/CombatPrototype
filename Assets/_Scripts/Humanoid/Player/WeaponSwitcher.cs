using UnityEngine;
using System;

public class WeaponSwitcher : MonoBehaviour
{
    public event Action<Weapon> OnNewWeapon;

    public Player player;
    public Weapon[] weapons;

    private int currentWeapon;

    private void Awake()
    {
        currentWeapon = 0;

        player.inputReader.OnNextWeapon += NextWeapon;
        player.inputReader.OnPreviousWeapon += PreviousWeapon;
    }

    private void Start()
    {
        for (int i = 1; i < weapons.Length; i++)
        {
            weapons[i].Hidden();
        }
        SetWeapon();
    }

    private void OnDisable()
    {
        player.inputReader.OnNextWeapon -= NextWeapon;
        player.inputReader.OnPreviousWeapon -= PreviousWeapon;
    }

    private void NextWeapon()
    {
        if (!player.CanSwitchWeapon())
        {
            return;
        }

        if (currentWeapon < weapons.Length - 1)
        {
            currentWeapon++;
        }
        else
        {
            currentWeapon = 0;
        }

        SetWeapon();
    }
    private void PreviousWeapon()
    {
        if (!player.CanSwitchWeapon())
        {
            return;
        }

        if (currentWeapon > 0)
        {
            currentWeapon--;
        }
        else
        {
            currentWeapon = weapons.Length - 1;
        }

        SetWeapon();
    }

    private void SetWeapon()
    {
        OnNewWeapon?.Invoke(GetCurrentWeapon());
    }


    public Weapon GetCurrentWeapon()
    {
        return weapons[currentWeapon];
    }
}
