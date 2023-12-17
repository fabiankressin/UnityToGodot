using UnityEngine;
using UnityEngine.UI;

public class ObjectNameDisplay : MonoBehaviour
{
    [SerializeField] private Text objectNameText;

    void Update()
    {
        if (MainGameManager.Instance.isGamePaused)
        {
            objectNameText.gameObject.SetActive(false);
            return;
        }
        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Check if the ray hits an object
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object has a name and InteractableObject component
            InteractableObject interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();

            if (interactableObject != null && !string.IsNullOrEmpty(interactableObject.displayName))
            {
                // Calculate the actual distance to the object
                float actualDistance = Vector3.Distance(Camera.main.transform.position, hit.point);

                // Check if the actual distance is within the interaction distance of the object
                if (actualDistance <= interactableObject.interactionDistance)
                {
                    // Display the object name in the text field
                    if (interactableObject.canBePickedUp)
                    {
                        objectNameText.text = interactableObject.displayName;// + "\n[F] Pick up";
                    }
                    else
                    {
                        objectNameText.text = interactableObject.displayName;
                    }
                    objectNameText.color = Color.white;
                    objectNameText.gameObject.SetActive(true);
                }
                else
                {
                    // Hide the text field if the actual distance is greater than the interaction distance
                    objectNameText.gameObject.SetActive(false);
                }
            }
            else
            {
                // Hide the text field if the hit object doesn't have a name or InteractableObject component
                objectNameText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Hide the text field if no object is hit
            objectNameText.gameObject.SetActive(false);
        }
    }
}
