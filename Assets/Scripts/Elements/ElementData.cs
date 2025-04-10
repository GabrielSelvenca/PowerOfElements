using UnityEngine;

public enum ElementType
{
    Water,
    Fire,
    Earth,
    Air
}

[System.Serializable]
public class ElementData
{
    public ElementType type;
    public LayerMask layer;
    public float initialDamage;
    public float continuousDamage;
    public float delay;
    public float speedModifier = 1f;
    public float force = 0f;
}