namespace _VRArchery.Scripts.Utility
{
    public static class Locator
    {
        private static class Cache<T> where T : class
        {
            public static T Instance { get; set; }
        }

        /// <summary>
        /// インスタンスを登録する
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>(T instance) where T : class => Cache<T>.Instance = instance;

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>() where T : class => Cache<T>.Instance;
    }
}