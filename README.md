# Yet Another WinAppDriver (YWinAppDriver)

[Microsoft WinAppDriver](https://github.com/Microsoft/WinAppDriver) is the official application  to support Selenium-like UI Test Automation on Windows Applications. This service supports testing Universal Windows Platform (UWP), Windows Forms (WinForms), Windows Presentation Foundation (WPF), and Classic Windows (Win32) apps on Windows 10 PCs.

This repo is an open source asp.net core implementation of WinAppDriver and it's compatible with Microsoft's WinAppDriver. Most of the ideas are coming from the test infrastructure of [WinUI](https://github.com/microsoft/microsoft-ui-xaml), [Microsoft.Windows.Apps.Test](https://github.com/Microsoft/Microsoft.Windows.Apps.Test), and [WinAppDriver document](https://github.com/microsoft/WinAppDriver/tree/master/Docs). I combined them and come up with the open source implementation. 

I name this project YWinAppDriver(yet another WinAppDriver).

## Project Status: 0.11
   Some basic functionality(Launch app, FindElement, FindElements, Click, DoubleClick, Value...) is ready. Please refer to [SessionController.cs](https://github.com/licanhua/YWinAppDriver/blob/main/src/WinAppDriver/Controllers/SessionController.cs)

   I successfully made [CalculatorTest](https://github.com/licanhua/YWinAppDriver/tree/main/examples/CalculatorTest) work which is come from https://github.com/microsoft/WinAppDriver/tree/master/Samples/C%23/CalculatorTest

## Not ready features
Below features are not ready yet
- logs
- xpath locator
- keyboard Input support. Part completed(/value endpoint) 
Similar functionality as WinUI did on [KeyboardHelper.cs](https://github.com/microsoft/microsoft-ui-xaml/blob/9b264ff73eeea18f6e13abe0b8ad9395b1c0138b/test/testinfra/MUXTestInfra/Common/KeyboardHelper.cs#L109)
- Mouse Input support. WinUI functionality in [InputHelper.cs](https://github.com/microsoft/microsoft-ui-xaml/blob/master/test/testinfra/MUXTestInfra/Common/InputHelper.cs)
- ExecuteScript

## Download & Run YWinAppDriver
- Download and compile 
There are two ways to get the WinAppDriver.exe:
1. Clone this repo, then open the WinAppDriver.sln and build/run WinAppDriver project.
2. or Download it from https://github.com/licanhua/YWinAppDriver/releases

- Lauch WinAppDriver.exe
Please set your test endpoint to http://127.0.0.1:4723
If you want to use other port and url, please change these lines and rebuild the project

If you launch it outside of Visual studio, run `WinAppDriver.exe --urls http://127.0.0.1/4723`

```
    webBuilder.UseUrls("http://localhost:4723");
```
and
```
    app.UsePathBase("/wd/hub");
```

- Build and run the CalcatorTest in [examples](https://github.com/licanhua/YWinAppDriver/tree/main/examples/CalculatorTest)
Please run the test, please make sure Calculator is in Standard mode.

## Knowledge for contributors
1. [Asp.Net Core](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1) 

This project is developed with Asp.Net Core and referred 3.1

2. [Microsoft.Windows.Apps.Test](https://github.com/Microsoft/Microsoft.Windows.Apps.Test)

In Windows, [UIAutomation](https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-overview) is the technology which test driver could use to manipulate the UI.  

Microsoft.Windows.Apps.Test is the core UI Automation library which is spinned from WinAppDriver, which allows user to interact with the testapp outside of WinAppDriver. [WinUI](https://github.com/microsoft/microsoft-ui-xaml/blob/9b264ff73eeea18f6e13abe0b8ad9395b1c0138b/test/testinfra/) is the first public user which implements its own automation without WinAppDriver.

Microsoft.Windows.Apps.Test has the public nuget and is binary open sourced.
YWinAppDriver is based on Microsoft.Windows.Apps.Test too and used this library to interact with the testapp.

Microsoft.Windows.Apps.Test documentation can be found here: [Microsoft.Windows.Apps.Test.chm](https://github.com/microsoft/Microsoft.Windows.Apps.Test/blob/master/docs/Microsoft.Windows.Apps.Test.chm).

3. Protocols
There are two protocols: [w3c webdriver](https://www.w3.org/TR/webdriver/) and [selenium JsonWire Protocol](https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol)
JsonWire is obselete, but still there a lot of client/server doen't support w3c specification. So YWinAppDriver is trying to match with both protocols

4. [Locators and capalibilites](https://github.com/microsoft/WinAppDriver/blob/master/Docs/AuthoringTestScripts.md) are the same/nearly the same with WinAppDriver.

## Background
One week ago, another team reached to me to ask some advice to help them choose the UI automation tool. It makes me think: Although WinAppDriver is the de fact tool recommended by Microsoft, Is WinAppDriver the right tool for everybody? I didn't see other open source option yet, so I spent one weekend to create the 0.1 release.

Two years ago, [hassanuz](https://github.com/hassanuz) was an active contributor on [WinAppDriver](https://github.com/Microsoft/WinAppDriver) and he started to promote the WinAppDriver usage in Microsoft. I was working on [WinUI](https://github.com/microsoft/microsoft-ui-xaml), and WinUI used [Microsoft.Windows.Apps.Test](https://github.com/Microsoft/Microsoft.Windows.Apps.Test). So we sit down together to see if I can adopt iWinAppDriver. After the conversation, I found we hit the [White box and Gray box dilemma](https://medium.com/reactive-hub/detox-vs-appium-ui-tests-in-react-native-2d07bf1e244f). WinUI is a gray box testing while WinAppDriver is blackbox testing. 

WinUI provides a lot of amazing features in its [test infrastucture](https://github.com/microsoft/microsoft-ui-xaml/blob/9b264ff73eeea18f6e13abe0b8ad9395b1c0138b/test/testinfra/MUXTestInfra/Infra/) which WinAppDriver doesn't support:
- [Wait.ForIdle](https://github.com/microsoft/microsoft-ui-xaml/search?p=2&q=Wait%3A%3AForIdle) is the killer feature to make the testing stable. When WinAppDriver saw the element, it doesn't mean UI is ready for interaction. Wait.ForIdle lets you know UI is ready to take user's input, so you will not run into the `unstable` situation that automation test case clicked the button in test pipeine, but there is no response, and I never reproduce the problem locally.
- Dump the visual tree when test failed
- Restart the application when launching failed, and kill the application when there is exception in test
- Pan, drag, scroll, and gamepad support.
- Speed. WinUI has thousand of test cases, and I want it be finished as soon as possbile. 

To adopt WinAppDriver, we need to resolve these problems first. Then we introduced the plugin mode into WinAppDriver> User can build their own business logic to the plugin, the WinAppDriver would load it in start up. So for WinUI, I can move the `Infra` part into the plugin. For native application, ExecuteScript is not used, so we can re-use it without any impact to the existing selenium clients. So the message flow look like this:

 Selenium Client ExecuteScript-> WinAppDriver -> WinUI Plugin -> UIA - TestApp

We finished the prototype. Because the legal concern and there is no business value from leader's aspect, we didn't make it into the end user.

I think YWinAppDriver is able to address above problems, and possible make every body happy.
