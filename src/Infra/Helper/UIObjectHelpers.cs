using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Windows.Apps.Test.Automation;
using Microsoft.Windows.Apps.Test.Foundation;

namespace WinAppDriver.Infra.Helper
{
    internal class UIObjectHelpers
    {
        public const string UWP_CLASS_NAME = "Windows.UI.Core.CoreWindow";
        public const string WINUI_CLASS_NAME = "WinUIDesktopWin32WindowClass";

        public static UIObject GetTopLevelUIObject(UIObject topWindow, string[] classNames,
            int timeoutInMilliseconds = 300)
        {
            UIObject element = null;
            var controlType = (ControlType)topWindow.GetProperty(UIProperty.Get("ControlType"));

            if (controlType != ControlType.Window)
            {
                throw new InvalidOperationException(
                    $"{nameof(topWindow)} isn't of type {ControlType.Window}.");
            }

            if (classNames.Any((className) =>
                    topWindow.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase)))
            {
                element = topWindow;
            }
            else
            {
                foreach (var className in classNames)
                {
                    var currentTimeout = timeoutInMilliseconds;

                    var windowCondition = UICondition.CreateFromClassName(className)
                        .AndWith(UICondition.CreateFromName(topWindow.Name));

                    while (currentTimeout > 0
                           && !topWindow.Children.TryFind(
                               windowCondition, out element))
                    {
                        Thread.Sleep(100);

                        currentTimeout -= 100;
                    }
                }

                if (element == null)
                {
                    throw new UIObjectNotFoundException(
                        $"Unable to find {string.Join("|", classNames)} in {topWindow}");
                }
            }

            return element;
        }

        public static void LogObjectTree(UIObject root)
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
    }
}