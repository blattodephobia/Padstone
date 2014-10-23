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
        
        private readonly bool IsTemplateReferenceType = default(T) == null;
        private Func<T> instanceProvider;
        private T[] poolInternal;
        private int nextObjectIndex;
        
        protected virtual void OnNewObjectAccessed()
        {
            if (this.NextObjectIndex == this.Capacity)
            {
                this.NextObjectIndex = 0;
                this.InitializePool();
            }
        }

        private T[] PoolInternal
        {
            get
            {
                this.OnNewObjectAccessed();
                return this.poolInternal;
            }
        }

        private int NextObjectIndex
        {
            get
            {
                return this.nextObjectIndex;
            }

            set
            {
                this.nextObjectIndex = value;
            }
        }

        public int Capacity { get; private set; }

        private void InitializePool()
        {
            this.poolInternal = new T[this.Capacity];
            if (this.IsTemplateReferenceType || this.instanceProvider != DefaultInstanceProvider)
            {
                for (int i = 0; i < this.poolInternal.Length; i++)
                {
                    this.poolInternal[i] = this.instanceProvider.Invoke();
                }
            }
        }

        public T GetInstance()
        {
            this.NextObjectIndex++;
            return this.PoolInternal[this.NextObjectIndex];
        }

        public Pool()
        {
            this.instanceProvider = DefaultInstanceProvider;
            this.InitializePool();
        }

        public Pool(Func<T> instanceProvider)
        {
            this.instanceProvider = instanceProvider;
            this.InitializePool();
        }
    }
}
