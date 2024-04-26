using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Player player;
    public Weapon[] weapons;

    private int currentWeapon;

    private void Awake()
    {
        currentWeapon = 0;
        SetWeapon();

        player.inputReader.OnNextWeapon += NextWeapon;
        player.inputReader.OnPreviousWeapon += PreviousWeapon;
    }

    private void Start()
    {
        for (int i = 1; i < weapons.Length; i++)
        {
            weapons[i].Hidden();
        }
    }

    private void OnDisable()
    {
        player.inputReader.OnNextWeapon -= NextWeapon;
        player.inputReader.OnPreviousWeapon -= PreviousWeapon;
    }

    private void NextWeapon()
    {
        if (!player.playerActions.CanSwitchWeapon())
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
        if (!player.playerActions.CanSwitchWeapon())
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
        player.playerActions.SetNewWeapon(weapons[currentWeapon]);
    }

}
