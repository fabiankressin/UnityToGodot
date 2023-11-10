using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeClickHandler : MonoBehaviour
{
    public GameObject logPrefab;
    [SerializeField] private float interactionDistance = 4f;
    private int clickCount = 0;
    private bool canReplace = false;
    private int breakCount = 10;

    // Expose the AudioSource and AudioClip in the Unity Editor
    [SerializeField] private AudioClip treeClickSound;
 private void OnMouseDown()
    {
        if (clickCount < breakCount)
        {
            clickCount++;

            if (treeClickSound != null)
            {
                // Play the sound directly from the object's AudioSource component
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.clip = treeClickSound;
                    audioSource.Play();
                }
            }
        }
        else if (clickCount >= breakCount && Vector3.Distance(transform.position, FirstPersonController.instance.transform.position) <= interactionDistance)
        {
            ReplaceTreeWithLog();
        }
    }

    private void ReplaceTreeWithLog()
    {
        if (treeClickSound != null)
        {
            // Play the sound before replacing the tree
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = treeClickSound;
                audioSource.Play();
            }
        }

        Instantiate(logPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}