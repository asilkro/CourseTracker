﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Dispatching;
using CourseTracker.Maui.Services;


namespace CourseTracker.Maui.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected IDispatcher Dispatcher => DispatcherProvider.Current.GetForCurrentThread();
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void InvokeOnMainThread(Action action)
        {
            if (Dispatcher.IsDispatchRequired)
            {
                Dispatcher.Dispatch(action);
            }
            else
            {
                action.Invoke();
            }
        }

    }
}
