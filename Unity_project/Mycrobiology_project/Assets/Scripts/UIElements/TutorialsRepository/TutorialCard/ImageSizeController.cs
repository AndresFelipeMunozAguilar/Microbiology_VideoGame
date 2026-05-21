using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSizeController : MonoBehaviour
{
    [SerializeField] private Vector2 _sizeRatio = new Vector2(0.5f, 0.5f);

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        image = GetComponent<Image>();

        ResizeImage();
    }

    private void ResizeImage()
    {
        float targetWidth = parentRect.rect.width * _sizeRatio.x;
        float targetHeight = parentRect.rect.height * _sizeRatio.y;

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            targetWidth
        );

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            targetHeight
        );
    }

}