using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityPMSManager
{
    public class UIInteractableButton : UIInteractableBaseButton
    {
        [Serializable]
        public class ImageInfo
        {
            public Image image;
            public Color notInteractableColor = Color.gray;
            public Sprite originalSprite { get; set; }
            public Color originalColor { get; set; }

            public void setOriginal()
            {
                if (null == image)
                    return;

                originalSprite = image.sprite;
                originalColor = image.color;
            }

            public void setInteractable(bool isInteractable)
            {
                if (null == image)
                    return;

                if (isInteractable)
                {
                    image.color = originalColor;
                }
                else
                {
                    image.color = notInteractableColor;
                }
            }
        }

        [Serializable]
        public class TextInfo
        {
            public TextSelector text;
            public Color notInteractableColor = Color.gray;
            public Color originalColor { get; set; }

            public void setOriginal()
            {
                if (null == text.gameObject)
                    return;

                originalColor = text.color;
            }

            public void setInteractable(bool isInteractable)
            {
                if (null == text.gameObject)
                    return;

                if (isInteractable)
                {
                    text.color = originalColor;
                }
                else
                {
                    text.color = notInteractableColor;
                }
            }
        }

        [SerializeField] protected List<ImageInfo> m_imageInfos = new List<ImageInfo>();
        [SerializeField] protected List<TextInfo> m_textInfos = new List<TextInfo>();

        protected override void Awake()
        {
            base.Awake();

            setInfoOriginals();
        }

        private void setInfoOriginals()
        {
            foreach (var imageInfo in m_imageInfos)
            {
                imageInfo.setOriginal();
            }

            foreach (var textInfo in m_textInfos)
            {
                textInfo.setOriginal();
            }
        }

        public override void setInteractable(bool isInteractable, bool isOnlyGraphicModify = false)
        {
            base.setInteractable(isInteractable, isOnlyGraphicModify);

            foreach (var imageInfo in m_imageInfos)
            {
                imageInfo.setInteractable(isInteractable);
            }

            foreach (var textInfo in m_textInfos)
            {
                textInfo.setInteractable(isInteractable);
            }
        }

        protected void setOriginalTextColor()
        {
            foreach (var textInfo in m_textInfos)
            {
                if (null == textInfo.text.gameObject)
                    continue;

                textInfo.text.color = textInfo.originalColor;
            }
        }

        protected void setText(int textInfoIndex, string text)
        {
            if (m_textInfos.Count <= textInfoIndex)
                return;
            if (null == m_textInfos[textInfoIndex].text.gameObject)
                return;

            m_textInfos[textInfoIndex].text.text = text;
        }
    }
}