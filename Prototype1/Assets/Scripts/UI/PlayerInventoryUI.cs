using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI Instance { get; private set; }

    [SerializeField] InventorySlotUIScript[] storageSlots;
    [SerializeField] InventorySlotUIScript mainHandSlot;
    [SerializeField] InventorySlotUIScript[] craftingSlots;
    [SerializeField] InventorySlotUIScript craftingResultSlot;

    public enum SlotType
    {
        Storage,
        MainHand,
        Crafting,
        CraftingResult
    }

    private string[] swordRecipe = { "Wood Pile", "Stone", "", "" };
    private string[] pickaxeRecipe = { "Wood Pile", "Stone", "Stone", "" };
    private validRecipes currentRecipe = validRecipes.none;
    private int selectedSlot = 0;
    private SlotType selectedSlotType;
    private bool isSlotSelected = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MainGameManager.Instance.OnInventoryAction += MainGameManager_OnInventoryAction;
        MainGameManager.Instance.OnPauseAction += MainGameManager_OnPauseAction;
        gameObject.SetActive(false);

        foreach (InventorySlotUIScript slot in storageSlots)
        {
            slot.SetSlotType(SlotType.Storage);
        }

        mainHandSlot.SetSlotType(SlotType.MainHand);

        foreach (InventorySlotUIScript slot in craftingSlots)
        {
            slot.SetSlotType(SlotType.Crafting);
        }

        craftingResultSlot.SetSlotType(SlotType.CraftingResult);
    }

    void OnDestroy()
    {
        Instance = null;
        MainGameManager.Instance.OnInventoryAction -= MainGameManager_OnInventoryAction;
        MainGameManager.Instance.OnPauseAction -= MainGameManager_OnPauseAction;
    }

    private void MainGameManager_OnPauseAction(object sender, System.EventArgs e)
    {
        if (MainGameManager.Instance.isInventoryOpen)
        {
            Hide();
        }
    }

    private void MainGameManager_OnInventoryAction(object sender, System.EventArgs e)
    {
        if (MainGameManager.Instance.isInventoryOpen)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        if (!MainGameManager.Instance.isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);
        }
    }

    //##############################################################################
    //InventoryStorage

    public bool IsSlotSelected()
    {
        return isSlotSelected;
    }

    public bool SelectedSlotMatches(SlotType slotType, int index)
    {
        return selectedSlot == index && selectedSlotType == slotType;
    }

    private void MarkChild(SlotType slotType, int index = 0)
    {
        InventorySlotUIScript slotScript = getSlotUIScript(slotType, index);
        slotScript.Mark();
    }

    private void UnMarkChild(SlotType slotType, int index = 0)
    {
        InventorySlotUIScript slotScript = getSlotUIScript(slotType, index);
        slotScript.UnMark();
    }

    public void SelectSlot(SlotType slotType, int index = 0)
    {
        if (IsSlotSelected())
        {
            if (SelectedSlotMatches(slotType, index))
            {

                UnMarkChild(slotType, index);

                isSlotSelected = false;
                return;
            }
            else
            {
                InventorySlotUIScript slotScript1 = getSlotUIScript(selectedSlotType, selectedSlot);
                InventorySlotUIScript slotScript2 = getSlotUIScript(slotType, index);

                string id1 = slotScript1.GetID();
                Sprite image1 = slotScript1.GetSlotImage();
                int count1 = slotScript1.GetCount();

                string id2 = slotScript2.GetID();
                Sprite image2 = slotScript2.GetSlotImage();
                int count2 = slotScript2.GetCount();


                //Crafting
                if (slotType == SlotType.CraftingResult || selectedSlotType == SlotType.CraftingResult)
                {
                    if (((slotType == SlotType.CraftingResult && id1 == "") || (selectedSlotType == SlotType.CraftingResult && id2 == "")))
                    {
                        switch (currentRecipe)
                        {
                            case validRecipes.sword:
                                {
                                    for (int i = 0; swordRecipe.Length > i; i++)
                                    {
                                        if (swordRecipe[i] != "")
                                        {
                                            craftingSlots[i].SetItemCount(-1);
                                        }
                                    }
                                    slotScript1.UpdateSlot(id2, image2, count2 - count1);
                                    slotScript2.UpdateSlot(id1, image1, count1 - count2);
                                    break;
                                }
                            case validRecipes.pickaxe:
                                {
                                    for (int i = 0; pickaxeRecipe.Length > i; i++)
                                    {
                                        if (pickaxeRecipe[i] != "")
                                        {
                                            craftingSlots[i].SetItemCount(-1);
                                        }
                                    }
                                    slotScript1.UpdateSlot(id2, image2, count2 - count1);
                                    slotScript2.UpdateSlot(id1, image1, count1 - count2);
                                    break;
                                }
                            case validRecipes.none:
                                {
                                    break;
                                }
                        }
                        currentRecipe = validRecipes.none;
                    }
                }
                else if (slotType == SlotType.Crafting && (id1 == id2 || id2 == "") && id1 != "" && count1 > 1)
                {
                    slotScript1.UpdateSlot(id1, image1, -1);
                    slotScript2.UpdateSlot(id1, image1, 1);
                }
                else if (selectedSlotType == SlotType.Crafting && (id1 == id2 || id1 == "") && id2 != "" && count2 > 1)
                {
                    slotScript1.UpdateSlot(id2, image2, 1);
                    slotScript2.UpdateSlot(id2, image2, -1);
                }
                else if (id1 == id2 && id1 != "")
                {
                    slotScript2.UpdateSlot(id1, image1, count1);
                    slotScript1.UpdateSlot("", null, -count1);
                }
                else
                {
                    slotScript1.UpdateSlot(id2, image2, count2 - count1);
                    slotScript2.UpdateSlot(id1, image1, count1 - count2);
                }

                UnMarkChild(selectedSlotType, selectedSlot);

                isSlotSelected = false;

                if (slotType == SlotType.MainHand || selectedSlotType == SlotType.MainHand)
                {
                    //Update Main Hand
                }
            }
        }
        else
        {
            MarkChild(slotType, index);
            selectedSlot = index;
            selectedSlotType = slotType;
            isSlotSelected = true;
        }

        if (slotType == SlotType.Crafting)
        {
            checkForValidRecipe();
        }


    }

    public InventorySlotUIScript getSlotUIScript(SlotType slotType, int index = 0)
    {
        switch (slotType)
        {
            case SlotType.Storage:
                {
                    return storageSlots[index];
                }
            case SlotType.MainHand:
                {
                    return mainHandSlot;
                }
            case SlotType.Crafting:
                {
                    return craftingSlots[index];
                }
            case SlotType.CraftingResult:
                {
                    return craftingResultSlot;
                }
        }
        Debug.Log("getSlotUIScript: slotType not found. THIS SHOULD NEVER HAPPEN!");
        return null;
    }


    public void SetImageAndCountOnSlot(string id, Sprite image, int count, int? slotIndex = null, SlotType slotType = SlotType.Storage)
    {
        if (slotType != SlotType.Storage)
        {
            Debug.Log("SetImageAndCountOnSlot: slotType != SlotType.Storage");
            return;
        }

        if (slotIndex != null)
        {
            InventorySlotUIScript slotScript = getSlotUIScript(slotType, slotIndex.Value);

            if (slotScript != null)
            {
                slotScript.UpdateSlot(id, image, count);
                return;
            }
        }
        else
        {
            // Iterate over all children and find the first slot with the same id
            foreach (InventorySlotUIScript slotScript in storageSlots)
            {
                // Check if the slot has the same image or an empty image
                if (slotScript != null && slotScript.GetID() == id)
                {
                    slotScript.UpdateSlot(id, image, count);
                    return;
                }
            }
            // Iterate over all children and find the first slot with an empty id
            foreach (InventorySlotUIScript slotScript in storageSlots)
            {
                // Check if the slot has the same image or an empty image
                if (slotScript != null && slotScript.SlotIsEmpty())
                {
                    slotScript.UpdateSlot(id, image, count);
                    return;
                }
            }
        }
    }

    //############################################################################
    //Crafting
    //Recipes
    enum validRecipes
    {
        sword,
        pickaxe,
        none
    }
    public void checkForValidRecipe()
    {
        craftingResultSlot.SetItemCount(-craftingResultSlot.GetCount());
        string[] slotContent = { "", "", "", "" };
        int slotCounter = 0;
        foreach (InventorySlotUIScript slotScript in craftingSlots)
        {
            slotContent[slotCounter] = slotScript.GetID();
            slotCounter++;
        }
        if (slotContent[0] == pickaxeRecipe[0] && slotContent[1] == pickaxeRecipe[1] && slotContent[2] == pickaxeRecipe[2] && slotContent[3] == "")
        {
            craftingResultSlot.UpdateSlot("pickaxe", Resources.Load<Sprite>("pickaxe"), 1);
            currentRecipe = validRecipes.pickaxe;
        }
        else if (slotContent[0] == swordRecipe[0] && slotContent[1] == swordRecipe[1] && slotContent[2] == "" && slotContent[3] == "")
        {
            craftingResultSlot.UpdateSlot("sword", Resources.Load<Sprite>("sword"), 1);
            currentRecipe = validRecipes.sword;
        }
        else
        {
            craftingResultSlot.UpdateSlot("", null, 0);
            currentRecipe = validRecipes.none;
        }
    }

}
