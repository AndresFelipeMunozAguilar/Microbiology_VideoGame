using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSizeController : MonoBehaviour
{
    [SerializeField] private Vector2 _sizeRatio = new Vector2(0.5f, 0.5f);

    private RectTransform _rectTransform;
    private RectTransform _parentRect;
    private Image image;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRect = transform.parent.GetComponent<RectTransform>();
        image = GetComponent<Image>();

        ResizeImage();
    }

    private void ResizeImage()
    {
        float targetWidth = _parentRect.rect.width * _sizeRatio.x;
        float targetHeight = _parentRect.rect.height * _sizeRatio.y;

        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            targetWidth
        );

        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            targetHeight
        );
    }

}