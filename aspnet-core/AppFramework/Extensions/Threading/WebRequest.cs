﻿using Abp.Runtime.Validation;
using Abp.UI;
using Flurl.Http;
using System;
using System.Threading.Tasks;
using Prism.Ioc;
using AppFramework.Localization;
using AppFramework.Extensions;
using AppFramework.Services;

namespace AppFramework.Common
{
    public static class WebRequest
    {
        private static readonly Lazy<IAppHostDialogService> dialogService =
            new Lazy<IAppHostDialogService>(
            ContainerLocator.Container.Resolve<IAppHostDialogService>
        );

        public static async Task Execute<TResult>(
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = _ => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                await successCallback(await func());
            }
            catch (Exception exception)
            {
                await HandleException(exception, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        public static async Task Execute(
            Func<Task> func,
            Func<Task> successCallback = null,
            Func<Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = () => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                await func();
                await successCallback();
            }
            catch (System.Exception ex)
            {
                await HandleException(ex, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        private static async Task HandleException(Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<Exception, Task> failCallback)
        {
            switch (exception)
            {
                case UserFriendlyException userFriendlyException:
                    await HandleUserFriendlyException(userFriendlyException, failCallback);
                    break;

                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;

                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;

                case AbpValidationException abpValidationException:
                    await HandleAbpValidationException(abpValidationException, failCallback);
                    break;

                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleException<TResult>(Exception exception,
           Func<Task<TResult>> func,
           Func<TResult, Task> successCallback,
           Func<Exception, Task> failCallback)
        {
            switch (exception)
            {
                case UserFriendlyException userFriendlyException:
                    await HandleUserFriendlyException(userFriendlyException, failCallback);
                    break;

                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;

                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;

                case AbpValidationException abpValidationException:
                    await HandleAbpValidationException(abpValidationException, failCallback);
                    break;

                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleUserFriendlyException(UserFriendlyException userFriendlyException,
           Func<Exception, Task> failCallback)
        {
            if (string.IsNullOrEmpty(userFriendlyException.Details))
                await dialogService.Value.Show(Local.Localize("Error"), userFriendlyException.Message);
            else
                await dialogService.Value.Show(userFriendlyException.Message, userFriendlyException.Details);

            await failCallback(userFriendlyException);
        }

        private static async Task HandleFlurlHttpTimeoutException<TResult>(
           FlurlHttpTimeoutException httpTimeoutException,
           Func<Task<TResult>> func,
           Func<TResult, Task> successCallback,
           Func<System.Exception, Task> failCallback)
        {
            var accepted = await dialogService.Value.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("RequestTimedOut"));

            if (accepted)
                await Execute(func, successCallback, failCallback);
            else
                await failCallback(httpTimeoutException);
        }

        private static async Task HandleFlurlHttpTimeoutException(FlurlHttpTimeoutException httpTimeoutException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = await dialogService.Value.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("RequestTimedOut"));

            if (accepted)
                await Execute(func, successCallback, failCallback);
            else
                await failCallback(httpTimeoutException);
        }

        private static async Task HandleFlurlHttpException(FlurlHttpException httpException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            if (await new ExceptionHandler().HandleIfAbpResponseAsync(httpException))
            {
                await failCallback(httpException);
                return;
            }

            var httpExceptionMessage = LocalTranslationHelper.Localize("HttpException");
            //if (Debugger.IsAttached)
            {
                httpExceptionMessage += Environment.NewLine + httpException.Message;
            }

            var accepted = await dialogService.Value.Question(
               LocalTranslationHelper.Localize("MessageTitle"),
               httpExceptionMessage);

            if (accepted)
                await Execute(func, successCallback, failCallback);
            else
                await failCallback(httpException);
        }

        private static async Task HandleFlurlHttpException<TResult>(FlurlHttpException httpException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            if (await new ExceptionHandler().HandleIfAbpResponseAsync(httpException))
            {
                await failCallback(httpException);
                return;
            }

            var httpExceptionMessage = LocalTranslationHelper.Localize("HttpException");
            //if (Debugger.IsAttached)
            {
                httpExceptionMessage += Environment.NewLine + httpException.Message;
            }

            var accepted = await dialogService.Value.Question(
              LocalTranslationHelper.Localize("MessageTitle"),
              httpExceptionMessage);

            if (accepted)
                await Execute(func, successCallback, failCallback);
            else
                await failCallback(httpException);
        }

        private static async Task HandleAbpValidationException(AbpValidationException abpValidationException,
            Func<System.Exception, Task> failCallback)
        {
            await dialogService.Value.Show(
                LocalTranslationHelper.Localize("MessageTitle"),
                abpValidationException.GetConsolidatedMessage());

            await failCallback(abpValidationException);
        }

        private static async Task HandleDefaultException(System.Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = await dialogService.Value.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("UnhandledWebRequestException"));

            if (accepted)
                await Execute(func, successCallback, failCallback);
            else
                await failCallback(exception);
        }

        private static async Task HandleDefaultException<TResult>(System.Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = await dialogService.Value.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("UnhandledWebRequestException"));

            if (accepted)
                await Execute(func, successCallback, failCallback);
            else
                await failCallback(exception);
        }
    }
}