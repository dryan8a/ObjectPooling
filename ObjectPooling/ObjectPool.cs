using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace ObjectPooling
{
    public class ObjectPool<T>
    {
        public static ObjectPool<T> Instance { get; private set; }

        static ObjectPool() { }

        public Action<T> ResetT { get; set; }
        public Func<T> CreateT { get; set; }

        private Queue<T> Pool;
        private HashSet<T> Used;
        class TComparer : IEqualityComparer<T>
        {
            public bool Equals([AllowNull] T x, [AllowNull] T y) => x.Equals(y);

            public int GetHashCode([DisallowNull] T obj) => RuntimeHelpers.GetHashCode(obj);
        }

        private ObjectPool(Func<T> CreateT, Action<T> ResetT, int initialPoolSize) 
        {
            this.CreateT = CreateT;
            this.ResetT = ResetT;
            Pool = new Queue<T>();
            for(int i = 0;i<initialPoolSize;i++)
            {
                Pool.Enqueue(CreateT());
            }
            Used = new HashSet<T>(new TComparer());
        }

        public static void InitializePool(Func<T> CreateT, Action<T> ResetT, int initialPoolSize)
        {
            Instance = new ObjectPool<T>(CreateT, ResetT, initialPoolSize);
        }

        public T Get()
        {
            var toBeGot = Pool.Count == 0 ? CreateT() : Pool.Dequeue();
            Used.Add(toBeGot);
            return toBeGot;
        }

        public bool Release(T Object)
        {
            if (!Used.Contains(Object)) return false;

            Used.Remove(Object);
            Pool.Enqueue(Object);

            return true;
        }

    }
}
