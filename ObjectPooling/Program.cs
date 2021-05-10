using System;

namespace ObjectPooling
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectPool<Cat>.InitializePool(() => new Cat(),(Cat x) => x.Name = "",10);
            ObjectPool<Giraffe>.InitializePool(() => new Giraffe(), (Giraffe x) => x.Length = 0, 5);

        }
    }
}
