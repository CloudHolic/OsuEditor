﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using OsuEditor.ViewModels;

namespace OsuEditor.Util
{
    public class CustomDialogManager : ICustomDialogManager
    {
        private readonly MetroDialogSettings _settings;

        public CustomDialogManager(MetroDialogSettings settings)
        {
            _settings = settings;
        }

        public async Task ShowDialogAsync(CustomDialog view)
        {
            var mainWindow = Application.Current.Windows.OfType<MetroWindow>().First();

            await mainWindow.ShowMetroDialogAsync(view, _settings);
            if (view.DataContext is DialogViewModelBase viewModel)
                await viewModel.Task;
            await mainWindow.HideMetroDialogAsync(view, _settings);
        }

        public Task ShowDialogAsync<TView>() where TView : CustomDialog
        {
            var view = Activator.CreateInstance<TView>();
            return ShowDialogAsync(view);
        }

        public async Task<TResult> ShowDialogAsync<TResult>(CustomDialog view)
        {
            var result = default(TResult);
            var mainWindow = Application.Current.Windows.OfType<MetroWindow>().First();

            await mainWindow.ShowMetroDialogAsync(view, _settings);
            if (view.DataContext is DialogViewModelBase<TResult> viewModel)
                result = await viewModel.Task;
            await mainWindow.HideMetroDialogAsync(view, _settings);

            return result;
        }

        public Task<TResult> ShowDialogAsync<TView, TResult>() where TView : CustomDialog
        {
            var view = Activator.CreateInstance<TView>();
            return ShowDialogAsync<TResult>(view);
        }

        public Task<MessageDialogResult> ShowMessageBox(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative,
            MetroDialogSettings settings = null)
        {
            var mainWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            return mainWindow.ShowMessageAsync(title, message, style, settings);
        }
    }
}
