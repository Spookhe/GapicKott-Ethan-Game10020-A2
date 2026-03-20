/* Ethan Gapic-Kott, 000923124 */

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public interface IElementInteractable
{
    void ApplyElement(InteractionSystem.ElementType element);
}

public class InteractableObject : MonoBehaviour, IElementInteractable
{
    [Header("State")]
    public InteractionSystem.ElementType currentState;

    [Header("Steam Interaction")]
    public GameObject steamGeyserPrefab;
    public AudioSource steamAudio;

    [Header("Explosion Interaction")]
    public GameObject explosionEffect;
    public GameObject objectToDestroy;
    public AudioSource explosionAudio; 

    [Header("Electrify Door Interaction")]
    public GameObject closedDoor;
    public GameObject openDoor;
    public AudioSource electrifyAudio;

    [Header("Cooldown Settings")]
    public float reactionCooldown = 2f;
    private bool isOnCooldown = false;

    void OnEnable()
    {
        ElementOrb.OnOrbImpact.AddListener(HandleImpact);
    }
    void OnDisable()
    {
        ElementOrb.OnOrbImpact.RemoveListener(HandleImpact);
    }

    // Only reacts if an object is hit
    void HandleImpact(InteractionSystem.ElementType element, GameObject target)
    {
        if (target != gameObject)
            return;

        ApplyElement(element);
    }

    public void ApplyElement(InteractionSystem.ElementType element)
    {
        // Ignores if an object is on cooldown
        if (isOnCooldown)
        {
            Debug.Log("Object is on cooldown, cannot react yet.");
            return;
        }

        // Uses current state to resolve element reactions
        string result = InteractionSystem.Instance.Resolve(currentState, element);
        Debug.Log(result);

        // Updates UI based on element reaction
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.UpdateUI(result);

        // Steam listeners
        if (result == "Steam")
        {
            if (steamGeyserPrefab != null)
                Instantiate(steamGeyserPrefab, transform.position + Vector3.up, Quaternion.identity);   // Creates a geyser prefab, controlled by GeyserLift.cs

            if (steamAudio != null)
                steamAudio.Play();
        }

        // Explosion listeners
        if (result == "Explosion")
        {
            if (explosionEffect != null)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);

            if (objectToDestroy != null)
                objectToDestroy.SetActive(false); // Destroys assigned gameobject

            if (explosionAudio != null)
                explosionAudio.Play();
        }

        // Electrify listeners
        if (result == "Electrify")
        {
            if (closedDoor != null)
                closedDoor.SetActive(false);

            if (openDoor != null)
                openDoor.SetActive(true);   // Opens door gameobjects by hiding/unhiding prefabs

            if (electrifyAudio != null)
                electrifyAudio.Play();
        }

        // Update current element state
        currentState = element;

        StartCoroutine(StartCooldown());
    }

    // Cooldown between element reactions (to avoid bugs)
    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(reactionCooldown);
        isOnCooldown = false;
    }
}