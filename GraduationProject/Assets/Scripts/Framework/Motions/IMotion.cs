using UnityEngine;
using UnityEngine.Events;

namespace GameFrameWork
{
    public interface IMotion
    {
        void InitTarget(GameObject target);
        void Play();
        void Stop();
        void AddOnFinished(UnityAction del);
        void RemoveOnFinished(UnityAction del);
        GameObject TweenTarget { get; }
    }

}
