using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeClickHandler : MonoBehaviour
{
    public GameObject logPrefab; // Drag the log prefab onto this field in the Inspector.
        [SerializeField] private float interactionDistance = 4f; // Integer variable that can be set in the Inspector.

    private int clickCount = 0;
    private bool canReplace = false;

    private void OnMouseDown()
    {
        if (clickCount < 4)
        {
            clickCount++;
        }
        else if (clickCount == 4 && Vector3.Distance(transform.position, FirstPersonController.instance.transform.position) <= interactionDistance)
        {
            ReplaceTreeWithLog();
        }
    }

    private void ReplaceTreeWithLog()
    {
        // Instantiate the log prefab at the same position and rotation as the tree.
        Instantiate(logPrefab, transform.position, transform.rotation);

        // Destroy the tree to remove it from the scene.
        Destroy(gameObject);
    }
}
