using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class ItemProperties : ScriptableObject
{
    public float Weight => weight;
    public float Volume => volume;
    public string Name => itemName;
    public string Description => description;

    [SerializeField] public float weight;
    [SerializeField] public float volume;
    [SerializeField] public string itemName;
    [SerializeField] public string description;

}