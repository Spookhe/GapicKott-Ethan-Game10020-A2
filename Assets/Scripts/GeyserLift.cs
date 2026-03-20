/* Ethan Gapic-Kott, 000923124 */

using UnityEngine;

public class GeyserLift : MonoBehaviour
{
    public float liftForce = 5f;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            // Lifts the player upwards while inside geyser
            if (cc != null)
                cc.Move(Vector3.up * liftForce * Time.deltaTime);
        }
    }
}