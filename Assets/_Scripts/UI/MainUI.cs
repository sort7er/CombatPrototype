using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;

    [SerializeField] private TextMeshProUGUI currentArchetypeText;

    private void Awake()
    {
        instance = this;
        SetWeaponText("");
    }


    public void SetWeaponText(string weapon)
    {
        currentArchetypeText.text = weapon;
    }

}
