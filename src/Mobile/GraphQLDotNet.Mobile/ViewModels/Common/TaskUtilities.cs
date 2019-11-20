using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace GraphQLDotNet.Mobile.ViewModels.Common
{
    public static class TaskUtilities
    {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForgetSafeAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
    }
}
