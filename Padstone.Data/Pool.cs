using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padstone.Data
{
    public class Pool<T>
    {
        private static Func<T> DefaultInstanceProvider = () => Activator.CreateInstance<T>();

        private Func<T> instanceProvider;
        private T[] poolInternal;
        private int capacity;
        private int nextObjectIndex;

        private bool isTemplateReferenceType = default(T) == null;

        private T[] PoolInternal
        {
            get
            {
                if (this.poolInternal == null || this.nextObjectIndex > this.Capacity - 1)
                {
                    this.InitializePool();
                }

                return this.poolInternal;
            }
        }

        public int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                this.capacity = value;
            }
        }

        private void InitializePool()
        {
            this.poolInternal = new T[this.Capacity];
            if (this.isTemplateReferenceType || this.instanceProvider != DefaultInstanceProvider)
            {
                for (int i = 0; i < this.poolInternal.Length; i++)
                {
                    this.poolInternal[i] = this.instanceProvider.Invoke();
                }
            }
        }

        public T GetInstance()
        {
            this.nextObjectIndex++;
            return this.PoolInternal[this.nextObjectIndex];
        }

        public Pool()
        {
            this.instanceProvider = DefaultInstanceProvider;
        }

        public Pool(Func<T> instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }
    }
}
