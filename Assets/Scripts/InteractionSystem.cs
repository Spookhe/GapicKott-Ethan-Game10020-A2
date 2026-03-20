/* Ethan Gapic-Kott, 000923124 */

using UnityEngine;
using System.Collections.Generic;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Instance;

    public enum ElementType
    {
        Fire,
        Water,
        Shock
    }

    [System.Serializable]   // Serialization for configuring elements combinations and their outcomes
    public class ElementInteraction
    {
        public ElementType existing;
        public ElementType incoming;
        public string result;
    }

    public List<ElementInteraction> interactions = new List<ElementInteraction>();

    void Awake()
    {
        Instance = this; // Allows global access for other scripts
    }

    // Returns the element reaction result for element combinations
    public string Resolve(ElementType existing, ElementType incoming)
    {
        foreach (var i in interactions)
        {
            if (i.existing == existing && i.incoming == incoming)
                return i.result;
        }

        return "No Reaction";
    }
}