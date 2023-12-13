using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class InventorySlotUIScript : MonoBehaviour
{
    private Image itemImage = null;
    private TextMeshProUGUI itemCountText = null;
    private string displayName = "";

    private void Start()
    {
        // Assuming the Image and TextMeshProUGUI components are the first and second children
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void initAttributes()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateSlotImage(Sprite itemSprite)
    {
        initAttributes();
        itemImage.sprite = itemSprite;
        itemImage.enabled = (itemSprite != null);
    }

    public void SetID(string id)
    {
        displayName = id;
    }

    
    public void SetItemCount(int count) // add count to current count
    {

        int currentCount = 0;

        // Attempt to parse the current text as an integer
        int.TryParse(itemCountText.text, out currentCount);

        // Increment the current count by 1
        currentCount += count;
        currentCount = Math.Max(currentCount, 0);

        if (currentCount == 0)
        {
            itemCountText.text = "";
            SetID("");
            UpdateSlotImage(null);
        }
        else
        {
            // Update the itemCountText with the new count
            itemCountText.text = currentCount.ToString();

            // Enable/disable based on the updated count
            itemCountText.enabled = (currentCount > 0);
        }

    }




    public Sprite GetSlotImage()
    {
        initAttributes();
        return itemImage.sprite;
    }

    public string GetID()
    {
        return displayName;
    }

    public int GetCount()
    {
        int currentCount = 0;

        // Attempt to parse the current text as an integer
        int.TryParse(itemCountText.text, out currentCount);
        return currentCount;
    }

    public bool SlotIsEmpty()
    {
        initAttributes();
        return displayName == "";
    }




    public void UpdateSlot(string id, Sprite image, int count)
    {
        SetID(id);
        UpdateSlotImage(image);
        SetItemCount(count);
    }
}
