using UnityEngine;
using UnityEngine.UI;

namespace Code.Scrips.UI
{
    public class CustomCursor : MonoBehaviour
    {
        public static CustomCursor instance; // Singleton for easy access


        private Image _image;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private RectTransform _rectTransform;

        void Start()
        {
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
            Debug.Log("set cursor");
            _image.sprite = newCursorSprite;
            //cursorImage.sprite = newCursorSprite;
            Cursor.visible = false;
        }
    }
}