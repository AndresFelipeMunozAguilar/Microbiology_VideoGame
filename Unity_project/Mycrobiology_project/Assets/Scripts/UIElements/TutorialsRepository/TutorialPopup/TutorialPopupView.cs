using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TutorialPopupView : MonoBehaviour
{
    [Header("Panel Contenedor Principal")]
    [Tooltip("El GameObject raíz de la ventana que contiene el fondo gris transparente.")]
    [SerializeField] private GameObject _rootPopupPanel;

    [Header("Componentes de Contenido")]
    [SerializeField] private Image _fullSizeTutorialImage;
    [SerializeField] private TextMeshProUGUI _titleText;

    [Header("Botones de Control")]
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        ValidateDependencies();

        if (_closeButton != null) _closeButton.onClick.AddListener(CloseWindow);

        CloseWindow();
    }

    public void OpenWindow(TutorialDataSO data)
    {
        if (data == null) Debug.LogError($"El parámetro {nameof(data)} es nulo.");

        // Rellenar la interfaz con los datos del ScriptableObject
        _fullSizeTutorialImage.sprite = data.FullTutorialSprite;
        _titleText.text = data.TutorialTitle;

        // Mostrar el canvas/panel
        _rootPopupPanel.SetActive(true);
    }

    // Cerrar y limpiar la ventana modal.
    public void CloseWindow()
    {
        _rootPopupPanel.SetActive(false);

        _fullSizeTutorialImage.sprite = null;
    }

    private void OnDestroy()
    {
        // Limpieza de listeners para evitar Memory Leaks (Fugas de memoria)
        if (_closeButton != null) _closeButton.onClick.RemoveListener(CloseWindow);
    }

    private void ValidateDependencies()
    {
        if (_rootPopupPanel == null) Debug.LogError($"Falta asignar el {nameof(_rootPopupPanel)} en el Visor.");
        if (_fullSizeTutorialImage == null) Debug.LogError($"Falta asignar el {nameof(_fullSizeTutorialImage)} en el Visor.");
        if (_titleText == null) Debug.LogError($"Falta asignar el {nameof(_titleText)} en el Visor.");
        if (_closeButton == null) Debug.LogError($"Falta asignar el {nameof(_closeButton)} en el Visor.");
    }
}