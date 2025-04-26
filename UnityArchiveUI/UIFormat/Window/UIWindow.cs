using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityPMSManager
{
    public class UIWindowData : UIWidgetData
    {
        public enum eAddPosition { First, Last }

        public eAddPosition addPosition = eAddPosition.First;
        public bool isMsgBox = false;
        public bool isDontDestroy = false;        
    }

    public class UIWindow : UIWidget
    {
        private bool m_isClosing = false;
        private bool m_isPlayingMotion = false;

        public bool isClosing { get { return m_isClosing; } }
        public bool isPlayingMotion => m_isPlayingMotion;

        public override void initialize(UIWidgetData data)
        {
            initMessage();

            base.initialize(data);

            var d = data as UIWindowData;
        }

        protected virtual void initMessage()
        {

        }

        public override void open()
        {
            base.open();

            m_isPlayingMotion = isAnimator;

            if (isAnimator)
            { 
                setActiveBlock(true);
                setMotion(openMotion);
            }
            else
            {
                setActiveBlock(false, false);
            }
        }

        public override void onClose()
        {
            m_isClosing = true;
            StopAllCoroutines();

            m_isPlayingMotion = isAnimator;

            if (isAnimator)
            {
                setActiveBlock(true);
                setMotion(closeMotion);
            }
        }

        public void onCloseImmediately()
        {
            m_isClosing = true;
            StopAllCoroutines();

            Dispose();
        }

        public virtual void resume(UIWindowData data)
        {
            gameObject.SetActive(true);
        }

        public virtual void suspend()
        {

        }

        /// <summary>
        /// close 될 때 navigation에 남아 있거나 isDontDestroy 상태 일 경우에 destroy 되지 않고 keep 함수가 호출 된다.
        /// </summary>
        public virtual void keep()
        {
            gameObject.SetActive(false);
        }

        public override void onAnimationOpenEndEvent()
        {
            m_isPlayingMotion = false;
            setActiveBlock(false);
        }

        public override void onAnimationCloseEndEvent()
        {
            m_isPlayingMotion = false;
            setActiveBlock(false);
            Dispose();
        }

        public virtual void back()
        {
            onClose();
        }
    }
}