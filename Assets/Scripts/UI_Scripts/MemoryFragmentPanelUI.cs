///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     MemoryFragmentPanelUI.cs
//
//  Author:     Izaiah Razon
//  Created:    04/14/2026
//  Updated:    04/14/2026
//
//  Description:
//  Displays memory fragment content and waits for player input to close.
//
//  Notes:
//  - Freezes gameplay while active.
//  - Closes on Enter key.
//  - Controlled by UIManager.
//  - Supports a customizable full-screen background overlay for readability.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryFragmentPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup panelGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Image backgroundImage;

    [Header("Overlay Settings")]
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.65f);

    private bool isOpen = false;
    private Action onCloseCallback;

    private void Start()
    {
        ApplyBackgroundColor();

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ClosePanel();
        }
    }

    private void OnValidate()
    {
        ApplyBackgroundColor();
    }

    public void OpenPanel(string title, string body, Action onClose)
    {
        if (panelGroup == null)
        {
            return;
        }

        isOpen = true;
        onCloseCallback = onClose;

        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;

        ApplyBackgroundColor();

        if (titleText != null)
        {
            titleText.text = title;
        }

        if (bodyText != null)
        {
            bodyText.text = body;
        }
    }

    private void ClosePanel()
    {
        isOpen = false;

        if (panelGroup != null)
        {
            panelGroup.alpha = 0f;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
        }

        onCloseCallback?.Invoke();
    }

    private void ApplyBackgroundColor()
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = backgroundColor;
        }
    }
}