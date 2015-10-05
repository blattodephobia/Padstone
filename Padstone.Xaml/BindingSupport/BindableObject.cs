using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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

		protected void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string callerMemberName = null)
		{
			backingField = value;
			this.OnPropertyChanged(callerMemberName);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
