using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameWork
{
    public class FallUIMotuin : BaseUIMotion
    {
        private bool _isForward;
        private float _duration;

        public FallUIMotuin(bool isForward, float duration = 1f)
        {
            _isForward = isForward;
            _duration = duration;
        }

        protected override void OnPlayHandle()
        {
            float moveY = (targetTransform as RectTransform).rect.height / 2 + Screen.height / 2;
            Tweener tweener;
            if (_isForward)
            {
                targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y + moveY, targetTransform.position.z);
                tweener = targetTransform.DOMoveY(targetTransform.position.y - moveY, _duration);
            }
            else
            {
                tweener = targetTransform.DOMoveY(targetTransform.position.y + moveY, _duration);
                tweener.onComplete += () =>
                {
                    targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y - moveY, targetTransform.position.z);
                };
            }
            tweener.SetUpdate(true);
            tweenerSequence = DOTween.Sequence();
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

