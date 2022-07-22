
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

using Command = MvvmHelpers.Commands.Command;

namespace ESATouristGuide.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetAndRaise(ref isBusy , value); }
        }

        private string title = string.Empty;
        private bool disposedValue;

        public string Title
        {
            get { return title; }
            set { SetAndRaise(ref title , value); }
        }

        //protected IUserLocationService UserLocationService { get; }
        //protected IDistancesService DistancesService { get; }
        //protected IWeatherService WeatherService { get; }

        public ICommand SkeletonCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public BaseViewModel()
        {

            BackCommand = new Command((x) =>
            {
                Shell.Current.SendBackButtonPressed();
            });
        }

        public virtual void OnNavigated(object parameter)
        {
        }

        /// <summary>
        /// Μπορεί να χρησιμοποιηθεί ως lazy load, δηλαδή να κληθεί μονάχα οταν θα είναι απαραίτητο ώστε να μην αργεί ο constructor του viewmodel.
        /// </summary>
        public virtual void Load()
        {

        }


        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Getting property name form expression is not supported for this type.");
            }

            if (!(expression is LambdaExpression lamda))
            {
                throw new NotSupportedException("Getting property name form expression is not supported for this type.");
            }

            if (lamda.Body is MemberExpression memberExpression)
            {
                RaisePropertyChanged(memberExpression.Member.Name);
                return;
            }

            UnaryExpression unary = lamda.Body as UnaryExpression;
            if (unary?.Operand is MemberExpression member)
            {
                RaisePropertyChanged(member.Member.Name);
                return;
            }

            throw new NotSupportedException("Getting property name form expression is not supported for this type.");
        }

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The set and raise.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool SetAndRaise<T>(ref T property , T value , [CallerMemberName] string propertyName = null)
        {
            if (Equals(property , value))
            {
                return false;
            }

            property = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseViewModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
