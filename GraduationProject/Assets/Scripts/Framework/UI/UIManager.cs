using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameWork
{
    public static class UISetting
    {
        public static string UICanvasPath = "Prefabs/UI/UICanvas";
        public static string EventSystemPath = "Prefabs/UI/EventSystem";
        public static string DarkModalPath = "Prefabs/UI/DarkModal";
        public static string WindowPath = "Prefabs/UI/Windows";
        public static string ViewPath = "Prefabs/UI/Views";
    }

    public static class UIManager
    {
        #region 属性
        private static ScreenName currentView = ScreenName.None;
        private static ScreenName currentWindow = ScreenName.None;

        public static ScreenName CurrentWindow => currentWindow;
        public static ScreenName CurrentView => currentView;
        public static GameObject UICanvas;
        public static DarkModal DarkModal;
        public static Transform WindowLayer;
        public static Transform ViewLayer;

        private static Dictionary<ScreenName, BaseWindow> catchWindows = new Dictionary<ScreenName, BaseWindow>();
        private static Dictionary<ScreenName, BaseView> catchViews = new Dictionary<ScreenName, BaseView>();

        private static Stack<ScreenName> showingWindowStack = new Stack<ScreenName>();
        #endregion

        #region 外界接口

        static UIManager()
        {
            GameObject obj = Resources.Load<GameObject>(UISetting.UICanvasPath);
            UICanvas = Object.Instantiate(obj);
            Object.DontDestroyOnLoad(UICanvas);
            obj = Resources.Load<GameObject>(UISetting.DarkModalPath);
            DarkModal = Object.Instantiate(obj).GetComponent<DarkModal>();
            Object.DontDestroyOnLoad(DarkModal);
            DarkModal.SetDarkModal(null, false);

            WindowLayer = UICanvas.transform.Find("WindowLayer");
            ViewLayer = UICanvas.transform.Find("ViewLayer");

            if(!GameObject.Find("EventSystem"))
            {
                obj = Resources.Load<GameObject>(UISetting.EventSystemPath);
                Object.Instantiate(obj);
            }

            EventManager.StartListening(EventName.OnScreenShowedHandle, OnScreenShowed);
            EventManager.StartListening(EventName.OnScreenClosedHandle, OnScreenClosed);
            EventManager.StartListening(EventName.OnScreenShowingHandle, OnScreenShowing);
            EventManager.StartListening(EventName.OnScreenClosingHandle, OnScreenClosing);
        }

        public static void OpenWindow(ScreenName screen, bool closeCurrent = false, bool useTween = true)
        {
            if (screen == currentWindow)
                return;
            if (closeCurrent && currentWindow != ScreenName.None)
            {
                CloseWindow(currentWindow, false);
            }
            currentWindow = screen;
            OpenOrCloseWindow(screen, true, useTween);
        }

        
        public static void CloseWindow(ScreenName screen, bool useTween = true)
        {
            if (!showingWindowStack.Contains(screen))
            {
//              Debug.LogError("Window:" + screen + "没有打开!");
                return;
            }
            OpenOrCloseWindow(screen, false, useTween);
        }

        public static void CloseView(ScreenName ScreenName, bool useTween = true) {
            if (ScreenName==currentView) {
                OpenOrCloseView(ScreenName, false, useTween);
            }
        }

        public static void OpenView(ScreenName ScreenName, bool useTween = true) {
            OpenOrCloseView(ScreenName, true, useTween);
        }

        public static void ChangeView(ScreenName screen, bool useTween = true)
        {
            if (screen == currentView)
                return;
            if (currentView != ScreenName.None)
            {
                OpenOrCloseView(currentView, false, useTween);
            }

            OpenOrCloseView(screen, true, useTween);
        }


        #endregion

        #region 内部实现

        private static void OpenOrCloseWindow(ScreenName screen, bool isOpen, bool useTween)
        {
            if (!screen.Equals(ScreenName.None))
            {
                BaseWindow window = GetWindow(screen);
                window.SetTopDepth();
                OpenOrCloseScreen(window, isOpen, useTween);
            }
        }

        private static void OpenOrCloseView(ScreenName screen, bool isOpen, bool useTween)
        {
            if (!screen.Equals(ScreenName.None))
            {
                BaseView view = GetView(screen);
                OpenOrCloseScreen(view, isOpen, useTween);
            }
        }

        private static void OpenOrCloseScreen(BaseScreen screen, bool isOpen, bool useTween = true)
        {
            if (screen != null)
            {
                if (isOpen)
                    screen.OpenScreen(useTween);
                else
                    screen.CloseScreen(useTween);
            }
            else
            {
                Debug.LogError("未找到UI预制体");
            }
        }


        private static BaseWindow GetWindow(ScreenName screen)
        {
            if (screen == ScreenName.None)
                return null;
            if (catchWindows.ContainsKey(screen))
            {
                return catchWindows[screen];
            }

            string path = UISetting.WindowPath + "/" + screen;
            GameObject go = LoadScreenInstance(screen.ToString(), path, WindowLayer);
            BaseWindow window = go.GetComponent<BaseWindow>();
            catchWindows.Add(screen, window);
            return window;
        }

        private static BaseView GetView(ScreenName screen)
        {
            if (catchViews.ContainsKey(screen))
            {
                return catchViews[screen];
            }

            string path = UISetting.ViewPath + "/" + screen;
            GameObject go = LoadScreenInstance(screen.ToString(), path, ViewLayer);
            BaseView view = go.GetComponent<BaseView>();

            catchViews.Add(screen, view);
            return view;
        }

        private static GameObject LoadScreenInstance(string screenName, string path, Transform parent)
        {
            GameObject obj = Resources.Load<GameObject>(path);
            obj = Object.Instantiate(obj, parent);
            obj.name = screenName;
            return obj;
        }
        #endregion
        #region 事件监听
        private static void OnScreenShowing()
        {
            object obj = EventManager.GetSender(EventName.OnScreenShowingHandle);
            BaseScreen screen = obj as BaseScreen;
            if (screen is BaseWindow)
                DarkModal.SetDarkModal(screen as BaseWindow, true);
        }

        private static void OnScreenClosing()
        {
            BaseScreen screen = EventManager.GetSender(EventName.OnScreenClosingHandle) as BaseScreen;
            if (screen is BaseWindow)
                DarkModal.SetDarkModal(screen as BaseWindow, false);
        }

        private static void OnScreenShowed()
        {
            BaseScreen screen = EventManager.GetSender(EventName.OnScreenShowedHandle) as BaseScreen;
            if(screen is BaseWindow)
            {
                showingWindowStack.Push(screen.gameObject.name.ToEnum<ScreenName>());
                currentWindow = showingWindowStack.Count == 0 ? ScreenName.None : showingWindowStack.Peek();
            }
            else if(screen is BaseScreen)
            {
                currentView = screen.gameObject.name.ToEnum<ScreenName>();
            }

        }

        private static void OnScreenClosed()
        {
            BaseScreen screen = EventManager.GetSender(EventName.OnScreenClosedHandle) as BaseScreen;
            if (screen is BaseWindow)
            {
                showingWindowStack.Pop();
                currentWindow = showingWindowStack.Count == 0 ? ScreenName.None : showingWindowStack.Peek();
                if(currentWindow != ScreenName.None && UIManager.GetWindow(currentWindow).IsModel)
                {
                    DarkModal.SetDarkModal(UIManager.GetWindow(currentWindow), true);
                }
            }
            else if (screen is BaseScreen)
            {
                currentView = ScreenName.None;
            }
        }
        #endregion
    }

}
