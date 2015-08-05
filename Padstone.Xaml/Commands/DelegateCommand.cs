using System;

namespace Padstone.Xaml
{
	/// <summary>
	/// Wraps a delegate of type <typeparamref name="System.Action"/> in an ICommand object that executes that delegate when ICommand.Execute is invoked.
	/// </summary>
    public class DelegateCommand : CommandBase
    {
        private static readonly Func<object, bool> CanAlwaysExecute = (obj) => { return true; };

        private Action<object> action;
        private Func<object, bool> canExecuteCallback;
        
        public override bool CanExecute(object parameter)
        {
            return this.canExecuteCallback.Invoke(parameter);
        }

        public override void Execute(object parameter)
        {
            if (this.action != null)
            {
                this.action.Invoke(parameter);
            }
        }

		/// <summary>
		/// Initializes a new <typeparamref name="Padstone.Xaml.DelegateCommand"/> that can always be executed.
		/// </summary>
		/// <param name="action">The method to be executed when ICommand.Execute is called.</param>
        public DelegateCommand(Action<object> action) :
            this(action, CanAlwaysExecute)
        {
        }

		/// <summary>
		/// Initializes a new <typeparamref name="Padstone.Xaml.DelegateCommand"/>
		/// </summary>
		/// <param name="action">The method to be executed when ICommand.Execute is called.</param>
		/// <param name="canExecuteCallback">The callback method that determines whether the <see cref="Execute"/> method should be called.</param>
        public DelegateCommand(Action<object> action, Func<bool> canExecuteCallback) :
			this(action, canExecuteCallback != null ? (obj) => canExecuteCallback.Invoke() : (Func<object, bool>)null)
        {
        }

		/// <summary>
		/// Initializes a new <typeparamref name="Padstone.Xaml.DelegateCommand"/>
		/// </summary>
		/// <param name="action">The method to be executed when ICommand.Execute is called.</param>
		/// <param name="canExecuteCallback">The callback method that determines whether the <see cref="Execute"/> method should be called.</param>
        public DelegateCommand(Action<object> action, Func<object, bool> canExecuteCallback)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
			}

			if (canExecuteCallback == null)
			{
				throw new ArgumentNullException("canExecuteCallback");
			}

            this.action = action;
            this.canExecuteCallback = canExecuteCallback;
        }
    }
}
