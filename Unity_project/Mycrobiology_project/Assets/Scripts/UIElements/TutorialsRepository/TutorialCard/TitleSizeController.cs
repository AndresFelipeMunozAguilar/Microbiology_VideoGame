using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TitleSizeController : MonoBehaviour
{
    [SerializeField] private float _fixedHeight = 80f;
    [SerializeField] private float _horizontalPadding = 20f;
    [SerializeField] private float _minFontSize = 15f;
    [SerializeField] private float _maxFontSize = 70f;

    private RectTransform _rectTransform;
    private RectTransform _parentRect;
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRect = transform.parent.GetComponent<RectTransform>();
        _tmp = GetComponent<TextMeshProUGUI>();

        ConfigureText();
        ResizeToParent();
    }

    private void ConfigureText()
    {
        _tmp.enableAutoSizing = true;

        _tmp.fontSizeMin = _minFontSize;
        _tmp.fontSizeMax = _maxFontSize;

        _tmp.overflowMode = TextOverflowModes.Ellipsis;

        _tmp.alignment = TextAlignmentOptions.Center;
    }

    private void ResizeToParent()
    {
        float parentWidth = _parentRect.rect.width;

        float targetWidth = parentWidth - _horizontalPadding;

        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            targetWidth
        );

        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            _fixedHeight
        );
    }

}