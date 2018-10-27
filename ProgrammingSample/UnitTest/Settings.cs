using System;

namespace UnitTest
{
    // 非静的クラスで Singleton。
    // プロパティへの初回アクセス時にインスタンスを初期化。
    // スレッドセーフではない。
    public class Settings1
    {
        static Settings1 _default;
        public static Settings1 Default
        {
            get
            {
                if (_default == null)
                    _default = new Settings1();
                return _default;
            }
        }

        Settings1() => Console.WriteLine($"{DateTime.Now:HH:mm:ss.fffffff}");
    }

    // プロパティへの初回アクセス時にインスタンスを初期化。
    // スレッドセーフ。
    public class Settings2
    {
        static readonly object _lock = new object();

        static Settings2 _default;
        public static Settings2 Default
        {
            get
            {
                if (_default == null)
                    lock (_lock)
                        if (_default == null)
                            _default = new Settings2();
                return _default;
            }
        }

        Settings2() => Console.WriteLine($"{DateTime.Now:HH:mm:ss.fffffff}");
    }

    // クラスの初期化時にインスタンスを初期化。
    // 静的コンストラクターを利用。
    public class Settings3
    {
        public static Settings3 Default { get; }

        static Settings3() => Default = new Settings3();

        Settings3() => Console.WriteLine($"{DateTime.Now:HH:mm:ss.fffffff}");
    }

    // クラスの初期化時にインスタンスを初期化。
    // 通常はこれを使えばよい。
    // Singleton の場合は、プロパティへの初回アクセス時 ≒ クラスの初期化時。
    public class Settings4
    {
        public static Settings4 Default { get; } = new Settings4();

        Settings4() => Console.WriteLine($"{DateTime.Now:HH:mm:ss.fffffff}");
    }

    // 静的クラス。
    public static class Settings5
    {
    }
}
