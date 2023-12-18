using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class InventorySlotUI : MonoBehaviour
{
    private Image itemImage = null;
    private TextMeshProUGUI itemCountText = null;
    private string displayName = "";

    private Button slotButton;

    public PlayerInventoryUI.SlotType slotType;


    private void Awake()
    {
        slotButton = GetComponent<Button>();

        if (slotButton != null)
        {
            slotButton.onClick.AddListener(() =>
            {
                pressButton();
            });
        }
        else
        {
            Debug.LogWarning("Button component not found on the GameObject.");
        }
    }

    void pressButton()
    {
        int index = transform.GetSiblingIndex();
        PlayerInventoryUI.Instance.SelectSlot(slotType, index);
    }

    private void Start()
    {
        initAttributes();
    }
    private void initAttributes()
    {
        // Assuming the Image and TextMeshProUGUI components are the first and second children
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateSlotImage(Sprite itemSprite)
    {
        initAttributes();
        itemImage.sprite = itemSprite;

        itemImage.enabled = (itemSprite != null);
        if (!itemImage.enabled)
        {
            Sprite emptySlotSprite = Resources.Load<Sprite>("empty_slot");
            itemImage.sprite = emptySlotSprite;
            itemImage.enabled = true;

        }
    }

    public void SetID(string id)
    {
        displayName = id;
    }


    public void AddToItemCount(int count)
    {

        int currentCount = 0;

        // Attempt to parse the current text as an integer
        int.TryParse(itemCountText.text, out currentCount);

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
            itemCountText.text = currentCount.ToString();

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

    public void Mark()
    {
        Color originalColor = itemImage.color;

        float desaturationFactor = 0.3f;
        Color desaturatedColor = Color.Lerp(originalColor, Color.gray, desaturationFactor);

        itemImage.color = desaturatedColor;
    }

    public void UnMark()
    {
        Color originalColor = Color.white;

        itemImage.color = originalColor;
    }


    public void UpdateSlot(string id, Sprite image, int count)
    {
        SetID(id);
        UpdateSlotImage(image);
        AddToItemCount(count);
    }

    public void ResetSlot()
    {
        itemCountText.text = "";
        SetID("");
        UpdateSlotImage(null);
    }

    public void SetSlotType(PlayerInventoryUI.SlotType type)
    {
        slotType = type;
    }
}
