using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private int clickCount = 0;
    [SerializeField] private int breakCount = 10;
    public float interactionDistance = 5f;
    [SerializeField] private AudioClip interactionSound;
    public bool canBePickedUp = false;
    [SerializeField] private bool dropItemsOnDestroy = true;
    [SerializeField] private int itemcount = 1;

    [SerializeField] public Sprite inventoryIcon;
    [SerializeField] public Sprite droppedInventoryIcon;
    [SerializeField] public GameObject droppedPrefab;
    [SerializeField] public string droppedName = "";
    [SerializeField] private bool useRigidbody = true;
    public string displayName = "";

    //public PlayerInventoryUI playerInventoryUI;
    [SerializeField] private bool initialObject = true;


    private void Start()
    {
        if (initialObject)
        {
            //playerInventoryUI = FindObjectOfType<PlayerInventoryUI>();

            // Check if the reference is null, and if so, print a warning
            if (PlayerInventoryUI.Instance == null)
            {
                Debug.LogWarning("PlayerInventoryUI component not found in the scene.");
            }
        }
        // Attempt to find the PlayerInventoryUI component in the scene

    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, FirstPersonController.instance.transform.position) <= interactionDistance)
        {
            if (interactionSound != null)
            {
                PlayInteractionSound();
            }
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        clickCount++;

        if (canBePickedUp)
        {
            PickUpObject();
            return;
        }

        if (clickCount >= breakCount)
        {
            DestroyObject();
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
            if (droppedName != "")
            {
                PlayerInventoryUI.Instance.SetImageAndCountOnSlot(droppedName, inventoryIcon, itemcount, null);
            }
            else
            {
                PlayerInventoryUI.Instance.SetImageAndCountOnSlot(displayName, inventoryIcon, itemcount, null);
            }
        }

        Destroy(gameObject);
    }



    private void DestroyObject()
    {
        if (dropItemsOnDestroy)
        {
            if (useRigidbody)
            {
                int numberOfObjects = Random.Range(5, 13);

                for (int i = 0; i < numberOfObjects; i++)
                {
                    // Calculate a random position within the bounds of the object's collider
                    Vector3 randomPosition = new Vector3(
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(0f, 2.5f),
                        Random.Range(-0.5f, 0.5f)
                    );

                    GameObject spawnedObject = Instantiate(droppedPrefab, transform.position + randomPosition, transform.rotation);
                    InteractableObject spawnedObjectScript = spawnedObject.GetComponent<InteractableObject>();
                    //spawnedObjectScript.playerInventoryUI = playerInventoryUI;
                    spawnedObjectScript.inventoryIcon = droppedInventoryIcon;

                    if (useRigidbody && spawnedObject.GetComponent<Rigidbody>() == null)
                    {
                        // Attach a Rigidbody if needed
                        spawnedObject.AddComponent<Rigidbody>();
                    }
                }
            }
            else
            {
                GameObject spawnedObject = Instantiate(droppedPrefab, transform.position, transform.rotation);
                InteractableObject spawnedObjectScript = spawnedObject.GetComponent<InteractableObject>();
                //spawnedObjectScript.playerInventoryUI = playerInventoryUI;
            }


        }
        Destroy(gameObject);
    }
}