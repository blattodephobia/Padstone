using System;
using System.Windows;
using System.Windows.Input;

namespace Padstone.Xaml
{
    public abstract class CommandBase : DependencyObject, ICommand
    {
        public static readonly DependencyProperty CustomCommandProperty =
            DependencyProperty.RegisterAttached(
            PropertyName.FromDependencyProperty(() => CustomCommandProperty),
            typeof(ICommand),
            typeof(CommandBase),
            new PropertyMetadata(null));

        public static ICommand GetCustomCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CustomCommandProperty);
        }

        public static void SetCustomCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CustomCommandProperty, value as ICommand);
        }

        public event EventHandler CanExecuteChanged;
        
        protected void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Forces subscribers to this command to call CanExecute again.
        /// </summary>
        public void InvalidateCanExecute()
        {
            this.RaiseCanExecuteChanged();
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
