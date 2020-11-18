using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PboViewer.ViewModels
{
    /// <summary>
    /// Main implementation of a MVVM command
    /// </summary>
    class RelayCommand : ICommand
    {
        #region Public Events

        /// <summary>
        /// Event fired when <see cref="CanExecute(object)"/> has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Private Members

        private Action _action;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayCommand(Action action) => _action = action;

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) => _action();

        #endregion
    }

    /// <summary>
    /// Main implementation of a MVVM command
    /// </summary>
    class RelayCommand<T> : ICommand
    {
        #region Public Events

        /// <summary>
        /// Event fired when <see cref="CanExecute(object)"/> has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Private Members

        private Action<T> _action;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayCommand(Action<T> action) => _action = action;

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter) 
        {
            object parameter1 = parameter;
            Type type = parameter1.GetType();
            if (type.FullName == "MS.Internal.NamedObject")
                return;

            if (!this.CanExecute(parameter1) || this._action == null)
                return;
            if (parameter1 == null)
            {
                if (typeof(T).GetTypeInfo().IsValueType)
                    _action(default(T));
                else
                    _action((T)parameter1);
            }
            else
                _action((T)parameter1);
        }

        #endregion
    }
}
