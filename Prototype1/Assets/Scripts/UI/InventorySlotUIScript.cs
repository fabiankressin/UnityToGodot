using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlotUIScript : MonoBehaviour
{
    private Image itemImage;
    private TextMeshProUGUI itemCountText;

    void Start()
    {
        // Assuming the Image and TextMeshProUGUI components are the first and second children
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateSlotImage(Sprite itemSprite)
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemImage.sprite = itemSprite;
        itemImage.enabled = (itemSprite != null);
    }

    public void SetItemCount(int count)
    {

        int currentCount = 0;

        // Attempt to parse the current text as an integer
        int.TryParse(itemCountText.text, out currentCount);

        // Increment the current count by 1
        currentCount += count;

        // Update the itemCountText with the new count
        itemCountText.text = currentCount.ToString();

        // Enable/disable based on the updated count
        itemCountText.enabled = (currentCount > 0);

    }
}
