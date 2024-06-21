using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrameWork
{
    public abstract class BaseUIMotion : IMotion
    {
        #region 属性和字段
        private GameObject tweenTarget;
        private UnityAction completeAction;
        private bool isPlaying;
        protected Sequence tweenerSequence;
        public GameObject TweenTarget
        {
            get { return tweenTarget; }
        }

        public Transform targetTransform
        {
            get { return TweenTarget.transform; }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
        }
        #endregion

        #region 外界接口
        public void InitTarget(GameObject target)
        {
            tweenTarget = target;
        }

        public void Play()
        {
            isPlaying = true;
            OnPlayHandle();
        }

        public void Stop()
        {
            if (!isPlaying) return;
            isPlaying = false;
            if (tweenTarget == null) return;
            OnStopHandle();
        }

        public void AddOnFinished(UnityAction callback)
        {
            completeAction += callback;
        }

        public void RemoveOnFinished(UnityAction callback)
        {
            completeAction -= callback;
        }
        #endregion

        protected void PlayComplete()
        {
            isPlaying = false;
            if (completeAction != null) completeAction();
        }

        #region 内部接口
        abstract protected void OnPlayHandle();
        abstract protected void OnStopHandle();
        #endregion

    }
}
