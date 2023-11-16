using UnityEngine;

[CreateAssetMenu(fileName = "New Archetype", menuName = "Archetype")]
public class Archetype : ScriptableObject
{
    public string archetypeName;

    public ArchetypePrefab archetypePrefab;

}
