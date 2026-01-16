using System;
using System.Threading;


public sealed class Singleton
{
    private static Singleton _instance;
    private static readonly object _lock = new object();

    private int _accessCount;   //счетчик количества обращений

    private Singleton()
    {
        _accessCount = 0;
    }

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)              // 1-я проверка
            {
                lock (_lock)                    //блокировка
                {
                    if (_instance == null)      // 2-я проверка
                    {
                        _instance = new Singleton(); //создали экземпляр
                    }
                }
            }

            return _instance;
        }
    }
    public void RegisterAccess() //при получении доступа к экземпляру делаем увеличение на 1
    {
        _accessCount++;
    }

    public int GetAccessCount() //по запросу выводим текущий счетчик обращений
    {
        return _accessCount;
    }
}



class Program
{
    static void Main()
    {
        Thread[] threads = new Thread[10];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(ThreadWork); //создаем потоки
            threads[i].Start();                     // запускаем потоки
        }


        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Все потоки завершены.");
    }

    static void ThreadWork()
    {
        Singleton.Instance.RegisterAccess();//обращаемся к классу
        int count = Singleton.Instance.GetAccessCount();//текущее значение счетчика
        Console.WriteLine(
            $"Поток {Thread.CurrentThread.ManagedThreadId}: " +
            $"количество обращений = {count}"
        );
    }
}

