using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance { get; private set; }

    [SerializeField] Image healthBar;
    [SerializeField] Image healthBarBackground;

    private PlayerInputActions playerInputActions;
    public event EventHandler OnPauseAction;
    public event EventHandler OnInventoryAction;
    public event EventHandler OnPlayerDeath;
    public bool isGamePaused = false;
    public bool isInventoryOpen = false;
    public bool isPlayerDead = false;

    public int playerHealth = 100;
    public int playerMaxHealth = 100;

    private void Start()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Inventory.performed += Inventory_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Inventory.performed -= Inventory_performed;
        playerInputActions.Dispose();
    }

    private void Inventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isInventoryOpen = !isInventoryOpen;
        OnInventoryAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        if (isPlayerDead)
        {
            return;
        }
        if (isInventoryOpen)
        {
            isInventoryOpen = false;
            OnInventoryAction?.Invoke(this, EventArgs.Empty);
        }

        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    //CALL THIS FUNCTION USING MainGameManager.Instance.DamagePlayer(int amount);
    public void DamagePlayer(int damageAmount)
    {
        if (damageAmount < 0)
        {
            return;
        }

        playerHealth -= damageAmount;

        // Calculate the health percentage
        float healthPercentage = (float)playerHealth / playerMaxHealth;

        // Update the health bar scale based on the percentage
        healthBar.transform.localScale = new Vector3(healthPercentage, 1f, 1f);

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            PlayerDeathEvent();
        }
    }

    //very hacky solution
    private void PlayerDeathEvent()
    {
        if (isInventoryOpen)
        {
            isInventoryOpen = false;
            OnInventoryAction?.Invoke(this, EventArgs.Empty);
        }

        if (isGamePaused)
        {
            isGamePaused = false;
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }
        healthBarBackground.enabled = false;
        healthBar.enabled = false;
        isGamePaused = !isGamePaused;
        Time.timeScale = 0f;
        isPlayerDead = true;
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    //CALL THIS FUNCTION USING MainGameManager.Instance.IsPlayerHealthFull();
    public bool IsPlayerHealthFull()
    {
        return playerHealth >= playerMaxHealth;
    }

    //CALL THIS FUNCTION USING MainGameManager.Instance.HealPlayer(int amount);
    public void HealPlayer(int healAmount)
    {
        if (healAmount < 0 && IsPlayerHealthFull())
        {
            return;
        }
        playerHealth += healAmount;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }

        // Calculate the health percentage
        float healthPercentage = (float)playerHealth / playerMaxHealth;

        // Update the health bar scale based on the percentage
        healthBar.transform.localScale = new Vector3(healthPercentage * 10, 1f, 1f);
    }

}
