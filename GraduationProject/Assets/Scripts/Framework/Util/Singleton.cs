public class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null) // ���̵߳ĵ���ʵ�ַ�ʽ
            {
                instance = new T();
            }

            return instance;
        }
    }
}