/* Ethan Gapic-Kott, 000923124 */

using UnityEngine;
using UnityEngine.Events;

public class ElementOrb : MonoBehaviour
{
    public InteractionSystem.ElementType elementType;

    [System.Serializable]
    public class OrbImpactEvent :
        UnityEvent<InteractionSystem.ElementType, GameObject>
    { }

    // Function for when an orb hits/collides with an object
    public static OrbImpactEvent OnOrbImpact = new OrbImpactEvent();

    void OnCollisionEnter(Collision collision)
    {
        // Notify listeners for the orbs element type
        OnOrbImpact.Invoke(elementType, collision.gameObject);

        Destroy(gameObject);
    }
}