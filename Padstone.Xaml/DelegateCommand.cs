using System;

namespace Padstone.Xaml
{
    public class DelegateCommand : CommandBase
    {
        private static readonly Func<bool> CanAlwaysExecute = () => { return true; };

        private Action<object> action;
        private Func<bool> canExecuteCondition;
        private Func<object, bool> canExecuteConditionParameterized;
        
        public override bool CanExecute(object parameter)
        {
            if (this.canExecuteCondition != null)
            {
                return this.canExecuteCondition.Invoke();
            }
            else if (this.canExecuteConditionParameterized != null)
            {
                return this.canExecuteConditionParameterized.Invoke(parameter);
            }
            else
            {
                return CanAlwaysExecute.Invoke();
            }
        }

        public override void Execute(object parameter)
        {
            if (this.action != null)
            {
                this.action.Invoke(parameter);
            }
        }

        public DelegateCommand(Action<object> action) :
            this(action, CanAlwaysExecute)
        {
        }

        public DelegateCommand(Action<object> action, Func<bool> canExecuteCondition)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;
            this.canExecuteCondition = canExecuteCondition;
        }

        public DelegateCommand(Action<object> action, Func<object, bool> canExecuteCondition)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;
            this.canExecuteConditionParameterized = canExecuteCondition;
        }
    }
}
