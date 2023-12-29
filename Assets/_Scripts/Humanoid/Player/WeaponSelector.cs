using System;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public event Action<Archetype> OnNewArchetype;

    [Header("References")]
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private Archetype[] archetypes;
    public Archetype currentArchetype { get; private set; }

    private InputReader inputReader;

    private int currentWeapon;
    private Archetype[] archetypePrefabs;

    private bool isHolstered;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();

        inputReader.OnNextWeapon += NextWeapon;
        inputReader.OnPreviousWeapon += PreviousWeapon;
        inputReader.OnHolster += Holster;

        archetypePrefabs = new Archetype[archetypes.Length];

        for (int i = 0; i < archetypes.Length; i++)
        {
            archetypePrefabs[i] = Instantiate(archetypes[i], weaponContainer);
            archetypePrefabs[i].gameObject.SetActive(false);
            isHolstered= true;
        }
        currentArchetype = archetypePrefabs[0];
    }
    private void OnDestroy()
    {
        inputReader.OnNextWeapon -= NextWeapon;
        inputReader.OnPreviousWeapon -= PreviousWeapon;
        inputReader.OnHolster -= Holster;
    }
    public void Holster()
    {
        if (currentArchetype.gameObject.activeSelf)
        {
            HideWeapon();
        }
        else if (!currentArchetype.gameObject.activeSelf)
        {
            ShowWeapon();
        }
    }
    private void NextWeapon()
    {
        if (currentArchetype.gameObject.activeSelf)
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
    private void PreviousWeapon()
    {
        if (currentArchetype.gameObject.activeSelf)
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

    public void ShowWeapon()
    {
        isHolstered= false;
        currentArchetype = archetypePrefabs[currentWeapon];
        currentArchetype.gameObject.SetActive(true);
        MainUI.instance.SetWeaponText(currentArchetype.archetypeName);
        OnNewArchetype?.Invoke(currentArchetype);
    }
    public void HideWeapon()
    {
        isHolstered = true;
        currentArchetype.archetypeAnimator.Abort();
        currentArchetype.gameObject.SetActive(false);
        MainUI.instance.SetWeaponText("");
    }
    public bool IsHolstered()
    {
        return isHolstered;
    }
}
