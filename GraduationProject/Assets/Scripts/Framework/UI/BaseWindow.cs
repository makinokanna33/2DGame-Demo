using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameWork
{
    public class BaseWindow : BaseScreen
    {
        [FoldoutGroup("BaseWindow属性")]
        public bool IsModel = true;
		[FoldoutGroup("BaseWindow属性")]
		public float ModalAlpha = 0.3f;

        protected virtual void Awake()
        {
			showMotion = new FallUIMotuin(true, 0.5f);
			closeMotion = new FallUIMotuin(false, 0.5f);
		}

        public virtual void OnDarkModalClicked() { }
		public void SetBottomDepth()
		{
			if (transform is RectTransform)
			{
				((RectTransform)transform).SetAsFirstSibling();
			}
		}

		public void SetTopDepth()
		{
			if (transform is RectTransform)
			{
				((RectTransform)transform).SetAsLastSibling();
			}
		}
    }
}
