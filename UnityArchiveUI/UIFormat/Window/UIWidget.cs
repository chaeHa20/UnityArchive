using System;
using System.Collections;
using UnityEngine;

namespace UnityPMSManager
{
    public class UIWidgetData
    {
        public string name = null;
    }

    public class UIWidget : UIComponent
    {
        protected enum eMotion
        {
            Idle = 0,
            Open = 1,
            Close = 2,
            FoldOut = 3,
            FoldIn = 4,
        }

        [SerializeField] GameObject m_block = null;

        private Animator m_animator = null;
        private Action m_destroyCallback = null;

        protected bool isAnimator { get { return null != m_animator; } }
        protected virtual int openMotion => (int)eMotion.Open;
        protected virtual int closeMotion => (int)eMotion.Close;

        public virtual void initialize(UIWidgetData data)
        {
            m_animator = GetComponent<Animator>();

            if (null != m_block)
                m_block.SetActive(false);
        }

        /// <summary>
        /// 초기화 후에 설정 되는 경우도 있어서 따로 함수를 만듬
        /// </summary>
        public void setDestroyCallback(Action destroyCallback)
        {
            m_destroyCallback = destroyCallback;
        }

        protected void setMotion(int motion)
        {
            if (isMotion(motion))
                return;

            m_animator.SetInteger("motion", motion);
        }

        protected bool isMotion(int motion)
        {
            if (null == m_animator)
                return false;

            if (m_animator.GetInteger("motion") == motion)
                return true;

            return false;
        }

        protected void playMotion(string stateName, int motion, float normalizedTime = 0.0f, bool isForce = false)
        {
            if (null == m_animator)
                return;

            if (!isForce)
            {
                if (m_animator.GetInteger("motion") == motion)
                    return;
            }

            m_animator.SetInteger("motion", motion);
            m_animator.Play(stateName, 0, normalizedTime);
        }

        protected int getMotion()
        {
            if (null == m_animator)
                return 0;

            return m_animator.GetInteger("motion");
        }

        public virtual void open()
        {
            gameObject.SetActive(true);
        }

        public virtual void onClose()
        {
        }

        public virtual void onClose(float delay)
        {
            if (0.0f < delay)
            {
                StartCoroutine(coClose(delay));
            }
            else
            {
                onClose();
            }
        }

        IEnumerator coClose(float delay)
        {
            setActiveBlock(true);
            yield return new WaitForSeconds(delay);
            setActiveBlock(false);

            onClose();
        }

        public virtual void refresh()
        {

        }

        public virtual void onAnimationOpenEndEvent()
        {

        }

        public virtual void onAnimationActiveContentEvent()
        {
        }

        public virtual void onAnimationCloseEndEvent()
        {

        }

        protected virtual void setActiveBlock(bool isBlock, bool isApplyAndroidBackButton = true)
        {
            if (null != m_block)
            {
                m_block.SetActive(isBlock);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                m_destroyCallback?.Invoke();

                GameObject.Destroy(gameObject);
            }
        }
    }
}