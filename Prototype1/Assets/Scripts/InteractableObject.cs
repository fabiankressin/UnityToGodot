using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private int clickCount = 0;
    [SerializeField] private int breakCount = 10;
    [SerializeField] private float interactionDistance = 4f;
    [SerializeField] private AudioClip interactionSound;
    [SerializeField] private bool canBePickedUp = false;
    [SerializeField] private bool dropItemsOnDestroy = true;
    [SerializeField] private GameObject inventoryIcon;
    [SerializeField] private GameObject droppedPrefab;
    [SerializeField] private bool useRigidbody = true;

    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, FirstPersonController.instance.transform.position) <= interactionDistance)
        {
            if (clickCount < breakCount)
            {
                HandleInteraction();
            }
            else if (clickCount >= breakCount)
            {
                DestroyObject();
            }
        }
    }

    private void HandleInteraction()
    {
        clickCount++;

        if (interactionSound != null)
        {
            PlayInteractionSound();
        }

        if (canBePickedUp)
        {
            PickUpObject();
        }
    }

    private void PlayInteractionSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = interactionSound;
            audioSource.Play();
        }
    }

    private void PickUpObject()
    {
        // Implement logic for picking up the object or placing it in the inventory
        // This can include spawning an inventory item, updating UI, etc.
        if (inventoryIcon != null)
        {
            // Instantiate inventory icon or add it to the player's inventory
            // Example: Instantiate(inventoryIcon, playerInventory.transform);
        }

        Destroy(gameObject);
    }

    private void DestroyObject()
    {
        if (interactionSound != null)
        {
            PlayInteractionSound();
        }


        if (dropItemsOnDestroy)
        {
            // Generate a random number of objects between 5 and 12
            int numberOfObjects = Random.Range(5, 13);

            for (int i = 0; i < numberOfObjects; i++)
            {
                // Calculate a random position within the bounds of the object's collider
                Vector3 randomPosition = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0f, 2.5f),
                    Random.Range(-0.5f, 0.5f)
                );

                // Spawn the object at the calculated position
                GameObject spawnedObject = Instantiate(droppedPrefab, transform.position + randomPosition, transform.rotation);

                if (useRigidbody && spawnedObject.GetComponent<Rigidbody>() == null)
                {
                    // Attach a Rigidbody if needed
                    spawnedObject.AddComponent<Rigidbody>();
                }
            }

            Destroy(gameObject);
        }
    }
}