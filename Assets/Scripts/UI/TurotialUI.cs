using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurotialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;

    [SerializeField] private OptionsUI optionsUI;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.GetState() == KitchenGameManager.State.WaitingForTutorialSkip)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = optionsUI.GetBindingText(OptionsUI.Binding.MoveUp);
        keyMoveDownText.text = optionsUI.GetBindingText(OptionsUI.Binding.MoveDown);
        keyMoveLeftText.text = optionsUI.GetBindingText(OptionsUI.Binding.MoveLeft);
        keyMoveRightText.text = optionsUI.GetBindingText(OptionsUI.Binding.MoveRight);
        keyInteractText.text = optionsUI.GetBindingText(OptionsUI.Binding.Interact);
        keyInteractAlternateText.text = optionsUI.GetBindingText(OptionsUI.Binding.InteractAlternate);
        keyPauseText.text = optionsUI.GetBindingText(OptionsUI.Binding.Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
