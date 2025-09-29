using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandartAssets.PlayerController;
public class StaminaController : MonoBehaviour
{
   
    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private PlayerController playerController;


    private void Awake()
    {
        playerController  = Object.FindFirstObjectByType<PlayerController>();
        if(playerController == null) 
            Debug.LogError("PlayerController not found in the scene");
    }

    private void Update()
    {
        if (playerController == null) return;

        float fillAmount = playerController.CurrentStamina / playerController.MaxStamina;
        staminaProgressUI.fillAmount = fillAmount;
    }
}
