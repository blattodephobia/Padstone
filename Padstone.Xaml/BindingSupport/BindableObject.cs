using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;

namespace Padstone.Xaml
{
	public abstract class BindableObject : INotifyPropertyChanged
	{
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChangedEvent = Volatile.Read(ref this.PropertyChanged);
			if (propertyChangedEvent != null)
			{
				propertyChangedEvent.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
		{
			this.OnPropertyChanged(PropertyName.Get(propertySelector));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
