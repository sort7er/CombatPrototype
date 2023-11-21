using UnityEngine;

public class Enemy : MonoBehaviour
{


    // This script is just to test the target assistance

    [Header("Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color targetColor;

    [Header("References")]
    [SerializeField] private MeshRenderer meshRenderer;

    private Material enemyMaterial;

    private void Awake()
    {
        enemyMaterial = meshRenderer.material;
        SetDefault();
    }


    public void SetAsTarget()
    {
        enemyMaterial.color = targetColor;
    }
    public void SetDefault()
    {
        enemyMaterial.color = defaultColor;
    }
}
