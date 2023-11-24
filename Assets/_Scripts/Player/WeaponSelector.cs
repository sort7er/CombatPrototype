using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    public event Action<ArchetypePrefab> OnNewArchetype;

    [Header("References")]
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private Archetype[] archetypes;

    private int currentWeapon;
    private ArchetypePrefab[] archetypePrefabs;

    private bool isHolstered;

    private void Awake()
    {
        archetypePrefabs = new ArchetypePrefab[archetypes.Length];
        for (int i = 0; i < archetypes.Length; i++)
        {
            archetypePrefabs[i] = Instantiate(archetypes[i].archetypePrefab, weaponContainer);
            archetypePrefabs[i].gameObject.SetActive(false);
            isHolstered= true;
        }
    }
    public void Holster(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (archetypePrefabs[currentWeapon].gameObject.activeSelf)
            {
                HideWeapon();
            }
            else if (!archetypePrefabs[currentWeapon].gameObject.activeSelf)
            {
                ShowWeapon();
            }
        }
    }
    public void NextWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (archetypePrefabs[currentWeapon].gameObject.activeSelf)
            {
                HideWeapon();

                if (currentWeapon < archetypePrefabs.Length - 1)
                {
                    currentWeapon++;
                }
                else
                {
                    currentWeapon = 0;
                }
            }

            ShowWeapon();
        }
    }
    public void PreviousWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (archetypePrefabs[currentWeapon].gameObject.activeSelf)
            {
                HideWeapon();

                if (currentWeapon > 0)
                {
                    currentWeapon--;
                }
                else
                {
                    currentWeapon = archetypePrefabs.Length - 1;
                }
            }

            ShowWeapon();
        }
    }

    public void ShowWeapon()
    {
        isHolstered= false;
        archetypePrefabs[currentWeapon].gameObject.SetActive(true);
        MainUI.instance.SetWeaponText(archetypes[currentWeapon].archetypeName);
        OnNewArchetype?.Invoke(archetypePrefabs[currentWeapon]);
    }
    public void HideWeapon()
    {
        isHolstered = true;
        archetypePrefabs[currentWeapon].Abort();
        archetypePrefabs[currentWeapon].gameObject.SetActive(false);
        MainUI.instance.SetWeaponText("");
    }
    public ArchetypePrefab CurrentArchetype()
    {
        return archetypePrefabs[currentWeapon];
    }
    public bool IsHolstered()
    {
        return isHolstered;
    }
}
