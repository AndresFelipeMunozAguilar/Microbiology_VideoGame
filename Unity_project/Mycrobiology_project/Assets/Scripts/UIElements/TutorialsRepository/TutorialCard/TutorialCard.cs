using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class TutorialCard : MonoBehaviour, IPointerClickHandler
{
    [Header("Componentes Visuales")]
    [SerializeField] private Image _thumbnailImage;
    [SerializeField] private Image _overlayLockImage;
    [SerializeField] private TextMeshProUGUI _titleText;

    // Dependencia de datos (Read-Only en runtime)
    private TutorialDataSO _tutorialData;
    private bool _isUnlocked;

    // Evento para notificar al visor central sin haber acoplamiento fuerte
    private Action<TutorialDataSO> _onCardSelectedCallback;

    /// Inicializa la tarjeta con sus datos y define su estado visual y de interacción.
    public void Setup(TutorialDataSO data, bool isUnlocked, Action<TutorialDataSO> onCardSelected)
    {
        _tutorialData = null;
        if (data == null) throw new ArgumentNullException(nameof(data)); else _tutorialData = data;

        _isUnlocked = isUnlocked;
        _onCardSelectedCallback = onCardSelected;

        RenderState();
    }


    private void RenderState()
    {
        if (_isUnlocked)
        {
            RenderUncoveredCard();
        }
        else
        {
            RenderCoveredCard();
        }
    }

    private void RenderUncoveredCard()
    {
        _thumbnailImage.sprite = _tutorialData.ThumbnailSprite;
        // Color normal para que la imagen se vea claramente
        _thumbnailImage.color = Color.white;
        _titleText.text = _tutorialData.TutorialTitle;
        _overlayLockImage.gameObject.SetActive(false);
    }

    private void RenderCoveredCard()
    {
        _thumbnailImage.sprite = _tutorialData.ThumbnailSprite;
        _titleText.text = "???";
        _overlayLockImage.gameObject.SetActive(true);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isUnlocked)
        {
            // Opcional: Aquí se podría disparar un evento acústico de "Bloqueado"
            return;
        }

        // Si está desbloqueado, notifica al listener (Visor Central) enviando sus datos
        _onCardSelectedCallback?.Invoke(_tutorialData);
    }
}