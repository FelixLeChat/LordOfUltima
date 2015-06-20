using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace LordOfUltima.Error
{
    class ErrorManager
    {
        private static ErrorManager _instance;
        public static ErrorManager Instance
        {
            get { return _instance ?? (_instance = new ErrorManager()); }
        }

        private readonly List<Error> _errorList = new List<Error>();
        public void AddError(Error error)
        {
            var errorInList = _errorList.FirstOrDefault(x => x.Description == error.Description);

            // if the error in not in the queue
            if (errorInList == null)
            {
                _errorList.Add(error);
            }
            else
            {
                // respawn the error
                errorInList.TimeRemaining = errorInList.TimeTotal;
            }
        }

        private Timer _ressourcesUpdateTimer;
        public void StartErrorDispatch()
        {
            _ressourcesUpdateTimer = new Timer(obj => { DecreaseErrorTime(); }, null, 0, 1000);
        }

        private void DecreaseErrorTime()
        {
            Error error = _errorList.FirstOrDefault(x => x.TimeRemaining > 0);
            if (error != null)
            {
                error.TimeRemaining--;
            }
        }

        public void DispatchErrors()
        {
            MainWindow mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            Error error = _errorList.FirstOrDefault(x => x.TimeRemaining > 0);
            if (error != null)
            {
                mainWindow.error_division.Visibility = Visibility.Visible;
                mainWindow.error_description.Content = error.GetDescriptionString();
            }
            else
            {
                mainWindow.error_division.Visibility = Visibility.Hidden;
            }
        }

    }
}
