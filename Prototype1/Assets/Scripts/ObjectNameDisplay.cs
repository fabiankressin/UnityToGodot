using UnityEngine;
using UnityEngine.UI;

public class ObjectNameDisplay : MonoBehaviour
{
    [SerializeField] private Text objectNameText;

    void Update()
    {
        if (MainGameManager.Instance.isGamePaused)
        {
            disableText();
            return;
        }
        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            InteractableObject interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
            if (interactableObject != null && !string.IsNullOrEmpty(interactableObject.displayName))
            {
                float actualDistance = Vector3.Distance(Camera.main.transform.position, hit.point);
                if (actualDistance <= interactableObject.interactionDistance)
                {
                    if (interactableObject.canBePickedUp)
                    {
                        objectNameText.text = interactableObject.displayName;// + "\n[F] Pick up";
                    }
                    else
                    {
                        objectNameText.text = interactableObject.displayName;
                    }
                    objectNameText.color = Color.white;
                    enableText();
                    return;
                }
            }
        }
        disableText();
    }
    private void enableText()
    {
        objectNameText.gameObject.SetActive(true);
    }
    private void disableText()
    {
        objectNameText.gameObject.SetActive(false);
    }
}
