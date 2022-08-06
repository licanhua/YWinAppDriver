using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Windows.Apps.Test.Automation;
using Microsoft.Windows.Apps.Test.Foundation;

namespace WinAppDriver.Infra.Helper
{
    internal class UIObjectHelpers
    {
        public const string WINUI_ERROR_MESSAGE_PREFIX = "Unable to find Windows.UI.Core.CoreWindow in ";
        public const string WINUI_CLASS_NAME = "WinUIDesktopWin32WindowClass";

        public static UIObject GetTopLevelUIObject(UICondition topLevelWindowCondition, string className)
        {
            if (!UIObject.Root.Descendants.TryFind(topLevelWindowCondition, out var element))
            {
                throw new InvalidOperationException(
                    $"{nameof(topLevelWindowCondition)} didn't match an element.");
            }

            return GetTopLevelUIObject(element, 5, className);
        }

        public static UIObject GetTopLevelUIObject(UIObject topWindow, int timeoutInMilliseconds, string className)
        {
            UIObject element = null;
            var controlType = (ControlType)topWindow.GetProperty(UIProperty.Get("ControlType"));

            if (controlType != ControlType.Window)
            {
                throw new InvalidOperationException(
                    $"{nameof(topWindow)} isn't of type {ControlType.Window}.");
            }

            if (topWindow.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase))
            {
                element = topWindow;
            }
            else
            {
                var windowCondition = UICondition.CreateFromClassName(className)
                    .AndWith(UICondition.CreateFromName(topWindow.Name));

                while (timeoutInMilliseconds > 0
                       && !topWindow.Children.TryFind(
                           windowCondition, out element))
                {
                    Thread.Sleep(100);

                    timeoutInMilliseconds -= 100;
                }

                if (element == null)
                    throw new UIObjectNotFoundException($"Unable to find {className} in {topWindow}");
            }

            return element;
        }

        public static void LogObjectTree(UIObject root = null)
        {
            LogObject(0, root);

            static void LogObject(int level, UIObject current)
            {
                if (level > 5)
                {
                    return;
                }

                var indent = new string(' ', level * 2);
                var name = OrDefault(current.Name, "(no name)");
                var className = OrDefault(current.ClassName, "(no class name)");
                var automationId = OrDefault(current.AutomationId, "(no automation id)");

                Debug.WriteLine($"{indent}{name} ({className}, {automationId})");

                foreach (var uiObject in current.Children)
                {
                    LogObject(level + 1, uiObject);
                }
            }

            static string OrDefault(string value, string defaultValue)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return defaultValue;
                }

                return value;
            }
        }


        public static (string name, string className) ParseErrorMessage(string errorMessage)
        {
            // Unable to find Windows.UI.Core.CoreWindow in {<name>, <class name>, }
            var context = errorMessage.Substring(WINUI_ERROR_MESSAGE_PREFIX.Length + 1);
            var parts = context.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                throw new Exception("Failed to identify winui name & class name");
            }

            return (parts[0].Trim(), parts[1].Trim());
        }
    }
}