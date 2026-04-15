///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     CodeInputPanelUI.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Displays a keyboard-driven code entry panel for locked doors.
//  Accepts digit input, Enter to submit, and Escape to cancel.
//
//  Notes:
//  - Controlled by UIManager.
//  - Freezes gameplay while active.
//  - Shows current input and status feedback.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeInputPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup panelGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image backgroundImage;

    [Header("Overlay Settings")]
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.65f);

    [Header("Input Settings")]
    [SerializeField] private int maxCodeLength = 4;
    [SerializeField] private string placeholderText = "----";

    private bool isOpen = false;
    private string currentInput = string.Empty;
    private Func<string, bool> submitCallback;
    private Action cancelCallback;

    private void Start()
    {
        ApplyBackgroundColor();

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
        }

        RefreshInputText();
    }

    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        HandleDigitInput();
        HandleBackspaceInput();
        HandleSubmitInput();
        HandleCancelInput();
    }

    private void OnValidate()
    {
        ApplyBackgroundColor();
    }

    public void OpenPanel(string panelTitle, Func<string, bool> onSubmit, Action onCancel)
    {
        if (panelGroup == null)
        {
            return;
        }

        isOpen = true;
        currentInput = string.Empty;
        submitCallback = onSubmit;
        cancelCallback = onCancel;

        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;

        ApplyBackgroundColor();

        if (titleText != null)
        {
            titleText.text = panelTitle;
        }

        if (statusText != null)
        {
            statusText.text = "Enter Access Code";
        }

        RefreshInputText();
    }

    public void ClosePanel()
    {
        isOpen = false;
        currentInput = string.Empty;
        submitCallback = null;
        cancelCallback = null;

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
        }

        RefreshInputText();
    }

    public void SetStatusMessage(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void HandleDigitInput()
    {
        if (currentInput.Length >= maxCodeLength)
        {
            return;
        }

        for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
        {
            if (Input.GetKeyDown(key))
            {
                currentInput += (key - KeyCode.Alpha0).ToString();
                RefreshInputText();
                return;
            }
        }

        for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
        {
            if (Input.GetKeyDown(key))
            {
                currentInput += (key - KeyCode.Keypad0).ToString();
                RefreshInputText();
                return;
            }
        }
    }

    private void HandleBackspaceInput()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            RefreshInputText();
        }
    }

    private void HandleSubmitInput()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }

        if (submitCallback != null)
        {
            bool wasCorrect = submitCallback.Invoke(currentInput);

            if (!wasCorrect)
            {
                currentInput = string.Empty;
                RefreshInputText();
            }
        }
    }

    private void HandleCancelInput()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }

        cancelCallback?.Invoke();
    }

    private void RefreshInputText()
    {
        if (inputText == null)
        {
            return;
        }

        inputText.text = string.IsNullOrEmpty(currentInput) ? placeholderText : currentInput;
    }

    private void ApplyBackgroundColor()
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = backgroundColor;
        }
    }
}