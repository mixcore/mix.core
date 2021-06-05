using System;

namespace Mix.Shared.Services
{
    public class SingletonService<T>
        where T: class
    {
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object syncRoot = new();

        /// <summary>
        /// The instance
        /// </summary>
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = (T)Activator.CreateInstance(typeof(T));
                        }
                    }
                }

                return instance;
            }
        }
    }
}
