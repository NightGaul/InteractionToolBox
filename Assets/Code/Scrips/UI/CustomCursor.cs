using UnityEngine;
using UnityEngine.UI;

namespace Code.Scrips.UI
{
    [RequireComponent(typeof(Image))]
    public class CustomCursor : MonoBehaviour
    {
        public static CustomCursor instance; // Singleton for easy access


        private Image _image;
        private RectTransform _rectTransform;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _image.enabled = false;
        }


        void Update()
        {
            Vector2 mousePos = Input.mousePosition;
            _rectTransform.position = mousePos;
        }

        public void SetCursor(Sprite newCursorSprite)
        {
            _image.enabled = true;
            _image.sprite = newCursorSprite;
            Cursor.visible = false;
        }
    }
}