using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameWork
{
    public class FadeUIMotion : BaseUIMotion
    {
        private bool _isForward;
        private float _duration;

        public FadeUIMotion(bool isForward, float duration = 1f)
        {
            _isForward = isForward;
            _duration = duration;
        }

        protected override void OnPlayHandle()
        {
            var fromAlpha = _isForward ? 0f : 1f;
            var toAlpha = _isForward ? 1f : 0f;

            CanvasGroup canvasGroup = targetTransform.GetComponent<CanvasGroup>();
            tweenerSequence = DOTween.Sequence();
            if (canvasGroup == null)
            {
                Debug.LogError("没有给TweenTarget添加CanvasGroup组件");
                return;
            }
            canvasGroup.alpha = fromAlpha;
            Tweener tweener = canvasGroup.DOFade(toAlpha, _duration);
            tweener.SetUpdate(true);
            tweenerSequence.Append(tweener);
            tweenerSequence.SetUpdate(true);
            tweenerSequence.onComplete += TweenComplete;
            tweenerSequence.Play();
        }

        private void TweenComplete()
        {
            PlayComplete();
        }

        protected override void OnStopHandle()
        {
            tweenerSequence.Kill();
        }
    }
}
