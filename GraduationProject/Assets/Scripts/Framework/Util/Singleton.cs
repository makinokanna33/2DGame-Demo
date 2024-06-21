public class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null) // 多线程的单例实现方式
            {
                instance = new T();
            }

            return instance;
        }
    }
}