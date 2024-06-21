using Sirenix.OdinInspector;
using UnityEngine;

namespace GameFrameWork
{
    public class BaseScreen : MonoBehaviour
    {
        protected bool isOpened = false;
        protected bool isShowed = false;
        protected bool isClosed = false;

        [FoldoutGroup("BaseScreen属性")]
        public GameObject Target;

        public IMotion showMotion;
        public IMotion closeMotion;

        protected IMotion ShowMotion
        {
            get
            {
                if (showMotion == null)
                    showMotion = new FadeUIMotion(true);
                return showMotion;
            }
        }

        protected IMotion CloseMotion
        {
            get
            {
                if (closeMotion == null)
                    closeMotion = new FadeUIMotion(false);
                return closeMotion;
            }
        }

        //TODO:开启Screen特效和关闭特效

        #region 外界接口

        public void OpenScreen(bool useTween)
        {
            if (isOpened) return;
            SetScreenShow(useTween);
        }

        public void CloseScreen(bool useTween)
        {
            if (!isOpened) return;
            SetScreenClose(useTween);
        }

        #endregion 外界接口

        #region 内部实现

        private void SetScreenShow(bool useTween)
        {
            isOpened = true;
            isShowed = false;
            isClosed = false;
            Target.SetActive(true);
            OnShowIng();

            EventManager.EmitEvent(EventName.OnScreenShowingHandle, this);

            ClearMotions();
            StopCloseMotion();
            if (useTween)
            {
                ShowMotion.InitTarget(Target);
                ShowMotion.AddOnFinished(Showed);
                ShowMotion.Play();
            }
            else
                Showed();
        }

        private void Showed()
        {
            isShowed = true;
            OnShowed();
            EventManager.EmitEvent(EventName.OnScreenShowedHandle, this);
        }

        private void SetScreenClose(bool useTween)
        {
            isOpened = false;
            isShowed = false;
            OnClosing();
            EventManager.EmitEvent(EventName.OnScreenClosingHandle, this);

            ClearMotions();
            StopShowMotion();
            if (useTween)
            {
                CloseMotion.InitTarget(Target);
                CloseMotion.AddOnFinished(Closed);
                CloseMotion.Play();
            }
            else
                Closed();
        }

        private void Closed()
        {
            isClosed = true;
            Target.SetActive(false);
            OnClosed();
            EventManager.EmitEvent(EventName.OnScreenClosedHandle, this);
        }

        private void ClearMotions()
        {
            if (ShowMotion != null) ShowMotion.RemoveOnFinished(Showed);
            if (CloseMotion != null) CloseMotion.RemoveOnFinished(Closed);
        }

        private void StopShowMotion()
        {
            if (ShowMotion != null)
            {
                ShowMotion.Stop();
                ShowMotion.RemoveOnFinished(Showed);
            }
        }

        private void StopCloseMotion()
        {
            if (CloseMotion != null)
            {
                CloseMotion.Stop();
                CloseMotion.RemoveOnFinished(Closed);
            }
        }

        #endregion 内部实现

        #region 内部接口

        protected virtual void OnShowIng()
        { }

        protected virtual void OnShowed()
        { }

        protected virtual void OnClosing()
        { }

        protected virtual void OnClosed()
        { }

        #endregion 内部接口
    }
}