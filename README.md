# Yet Another WinAppDriver (YWinAppDriver)

[Microsoft WinAppDriver](https://github.com/Microsoft/WinAppDriver) is the official application  to support Selenium-like UI Test Automation on Windows Applications. This service supports testing Universal Windows Platform (UWP), Windows Forms (WinForms), Windows Presentation Foundation (WPF), and Classic Windows (Win32) apps on Windows 10 PCs.

This repo is an open source asp.net core implementation of WinAppDriver and it's compatible with Microsoft's WinAppDriver. Most of the ideas are coming from the test infrastructure of [WinUI](https://github.com/microsoft/microsoft-ui-xaml), [Microsoft.Windows.Apps.Test](https://github.com/Microsoft/Microsoft.Windows.Apps.Test), and [WinAppDriver document](https://github.com/microsoft/WinAppDriver/tree/master/Docs). I combined them and come up with the open source implementation. 

I name this project YWinAppDriver(yet another WinAppDriver).

## Project Status: 0.2.1
   Some basic functionality(Launch app, FindElement, FindElements, Click, DoubleClick, Value...) is ready. Please refer to [SessionController.cs](https://github.com/licanhua/YWinAppDriver/blob/main/src/WinAppDriver/Controllers/SessionController.cs)

   I successfully made [CalculatorTest](https://github.com/licanhua/YWinAppDriver/tree/main/examples/CalculatorTest) work which is come from https://github.com/microsoft/WinAppDriver/tree/master/Samples/C%23/CalculatorTest
  - keyboard Input support. completed. Similar functionality as WinUI did on [KeyboardHelper.cs](https://github.com/microsoft/microsoft-ui-xaml/blob/9b264ff73eeea18f6e13abe0b8ad9395b1c0138b/test/testinfra/MUXTestInfra/Common/KeyboardHelper.cs#L109)
  - logs. Complete

Below is the table to match with [selenium json wire protocol](https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol#command-summary).
Because YWinAppDriver is for desktop application other than browser, `no` below means it's not supported.
`maybe` means it's possible to support it, but I didn't see any value to support it. 

| Dev Status | HTTP Method |	Path	| Summary
|---|---|---|---|
| completed |GET	|/status    |	Query the server's current status.
| completed |POST   |/session   |	Create a new session.
| completed |GET    |/sessions  |	Returns a list of the currently active sessions.
| in progress|GET	|/session/:sessionId	|   Retrieve the capabilities of the specified session.
| completed |DELETE |/session/:sessionId	|Delete the session.
|completed  |POST   |	/session/:sessionId/timeouts	|Configure the amount of time that a particular type of operation can execute for before they are aborted and a
| no|POST  |	/session/:sessionId/timeouts/async_script	|Set the amount of time, in milliseconds, that asynchronous scripts executed by /session/:sessionId/execute_async are permitted to run before they are aborted and a
|completed  |POST	|/session/:sessionId/timeouts/implicit_wait	|Set the amount of time the driver should wait when searching for elements.
|completed  |GET	|/session/:sessionId/window_handle	|Retrieve the current window handle.
| completed|GET|	/session/:sessionId/window_handles|	Retrieve the list of all window handles available to the session.
|no|GET    |	/session/:sessionId/url|	Retrieve the URL of the current page.
|no |POST|	/session/:sessionId/url	|Navigate to a new URL.
|maybe  |POST	|/session/:sessionId/forward |	Navigate forwards in the browser history, if possible.
|maybe|POST	|/session/:sessionId/back	|Navigate backwards in the browser history, if possible.
|maybe|POST	|/session/:sessionId/refresh	|Refresh the current page.
|maybe|POST	|/session/:sessionId/execute	|Inject a snippet of JavaScript into the page for execution in the context of the currently selected frame.
|maybe|POST|	/session/:sessionId/execute_async	|Inject a snippet of JavaScript into the page for execution in the context of the currently selected frame.
|maybe|GET|	/session/:sessionId/screenshot	|Take a screenshot of the current page.
|no|GET|	/session/:sessionId/ime/available_engines	|List all available engines on the machine.
|no|GET|	/session/:sessionId/ime/active_engine	|Get the name of the active IME engine.
|no|GET|	/session/:sessionId/ime/activated	|Indicates whether IME input is active at the moment (not if it's available.
|no|POST|	/session/:sessionId/ime/deactivate	|De-activates the currently-active IME engine.
|no|POST|	/session/:sessionId/ime/activate	|Make an engines that is available (appears on the listreturned by getAvailableEngines) active.
|no|POST|	/session/:sessionId/frame	|Change focus to another frame on the page.
|no|POST|	/session/:sessionId/frame/parent	|Change focus to the parent context.
|completed|POST|	/session/:sessionId/window	|Change focus to another window.
|completed|DELETE|	/session/:sessionId/window	|Close the current window.
|in progress|POST|	/session/:sessionId/window/:windowHandle/size	|Change the size of the specified window.
|in progress|GET|	/session/:sessionId/window/:windowHandle/size	|Get the size of the specified window.
|in progress|POST|	/session/:sessionId/window/:windowHandle/position	|Change the position of the specified window.
|in progress|GET|	/session/:sessionId/window/:windowHandle/position	|Get the position of the specified window.
|in progress|POST|	/session/:sessionId/window/:windowHandle/maximize	|Maximize the specified window if not already maximized.
|no|GET|	/session/:sessionId/cookie	|Retrieve all cookies visible to the current page.
|no|POST|	/session/:sessionId/cookie	|Set a cookie.
|no|DELETE|	/session/:sessionId/cookie	|Delete all cookies visible to the current page.
|no|DELETE|	/session/:sessionId/cookie/:name	|Delete the cookie with the given name.
|completed|GET|	/session/:sessionId/source	|Get the current page source.
|completed|GET|	/session/:sessionId/title	|Get the current page title.
|completed|POST|	/session/:sessionId/element	|Search for an element on the page, starting from the document root.
|completed|POST|	/session/:sessionId/elements	|Search for multiple elements on the page, starting from the document root.
|completed|POST|	/session/:sessionId/element/active	|Get the element on the page that currently has focus.
|completed|GET|	/session/:sessionId/element/:id	|Describe the identified element.
|completed|POST|	/session/:sessionId/element/:id/element	|Search for an element on the page, starting from the identified element.
|completed|POST|	/session/:sessionId/element/:id/elements	|Search for multiple elements on the page, starting from the identified element.
|completed|POST|	/session/:sessionId/element/:id/click	|Click on an element.
|no|POST|	/session/:sessionId/element/:id/submit	|Submit a FORM element.
|completed|GET|	/session/:sessionId/element/:id/text	|Returns the visible text for the element.
|completed|POST|	/session/:sessionId/element/:id/value	|Send a sequence of key strokes to an element.
|completed|POST|	/session/:sessionId/keys	|Send a sequence of key strokes to the active element.
|complented|GET|	/session/:sessionId/element/:id/name	|Query for an element's tag name.
|completed|POST|	/session/:sessionId/element/:id/clear	|Clear a TEXTAREA or text INPUT element's value.
|completed|GET|	/session/:sessionId/element/:id/selected	|Determine if an OPTION element, or an INPUT element of type checkbox or radiobutton is currently selected.
|completed|GET|	/session/:sessionId/element/:id/enabled	|Determine if an element is currently enabled.
|completed|GET|	/session/:sessionId/element/:id/attribute/:name	|Get the value of an element's attribute.
|completed|GET|	/session/:sessionId/element/:id/equals/:other	|Test if two element IDs refer to the same DOM element.
|completed|GET|	/session/:sessionId/element/:id/displayed	|Determine if an element is currently displayed.
|completed|GET|	/session/:sessionId/element/:id/location	|Determine an element's location on the page.
|maybe|GET|	/session/:sessionId/element/:id/location_in_view	|Determine an element's location on the screen once it has been scrolled into view.
|completed|GET|	/session/:sessionId/element/:id/size	|Determine an element's size in pixels.
|no|GET|	/session/:sessionId/element/:id/css/:propertyName	|Query the value of an element's computed CSS property.
|no|GET|	/session/:sessionId/orientation	|Get the current browser orientation.
|no|POST|	/session/:sessionId/orientation	|Set the browser orientation.
|no|GET|	/session/:sessionId/alert_text	|Gets the text of the currently displayed JavaScript alert(), confirm(), or prompt() dialog.
|no|POST|	/session/:sessionId/alert_text	|Sends keystrokes to a JavaScript prompt() dialog.
|no|POST|	/session/:sessionId/accept_alert	|Accepts the currently displayed alert dialog.
|no|POST|	/session/:sessionId/dismiss_alert	|Dismisses the currently displayed alert dialog.
|in progress|POST|	/session/:sessionId/moveto	|Move the mouse by an offset of the specificed element.
|completed|POST|	/session/:sessionId/click	|Click any mouse button (at the coordinates set by the last moveto command).
|in progress|POST|	/session/:sessionId/buttondown	|Click and hold the left mouse button (at the coordinates set by the last moveto command).
|in progress|POST|	/session/:sessionId/buttonup	|Releases the mouse button previously held (where the mouse is currently at).
|in progress|POST|	/session/:sessionId/doubleclick	|Double-clicks at the current mouse coordinates (set by moveto).
|in progress|POST|	/session/:sessionId/touch/click	|Single tap on the touch enabled device.
|in progress|POST|	/session/:sessionId/touch/down	|Finger down on the screen.
|in progress|POST|	/session/:sessionId/touch/up	|Finger up on the screen.
|in progress|POST|	session/:sessionId/touch/move	|Finger move on the screen.
|in progress|POST|	session/:sessionId/touch/scroll	|Scroll on the touch screen using finger based motion events.
|in progress|POST|	session/:sessionId/touch/scroll	|Scroll on the touch screen using finger based motion events.
|in progress|POST|	session/:sessionId/touch/doubleclick	|Double tap on the touch screen using finger motion events.
|in progress|POST|	session/:sessionId/touch/longclick	|Long press on the touch screen using finger motion events.
|in progress|POST|	session/:sessionId/touch/flick	|Flick on the touch screen using finger motion events.
|in progress|POST|	session/:sessionId/touch/flick	|Flick on the touch screen using finger motion events.
|no|GET	|/session/:sessionId/location	|Get the current geo location.
|no|POST|	/session/:sessionId/location	|Set the current geo location.
|no|GET|	/session/:sessionId/local_storage	|Get all keys of the storage.
|no|POST|	/session/:sessionId/local_storage	|Set the storage item for the given key.
|no|DELETE|	/session/:sessionId/local_storage	|Clear the storage.
|no|GET|	/session/:sessionId/local_storage/key/:key	|Get the storage item for the given key.
|no|DELETE|	/session/:sessionId/local_storage/key/:key	|Remove the storage item for the given key.
|no|GET|	/session/:sessionId/local_storage/size	|Get the number of items in the storage.
|no|GET|	/session/:sessionId/session_storage	|Get all keys of the storage.
|no|POST|	/session/:sessionId/session_storage	|Set the storage item for the given key.
|no|DELETE|	/session/:sessionId/session_storage	|Clear the storage.
|no|GET|	/session/:sessionId/session_storage/key/:key	|Get the storage item for the given key.
|no|DELETE|	/session/:sessionId/session_storage/key/:key	|Remove the storage item for the given key.
|no|GET|	/session/:sessionId/session_storage/size	|Get the number of items in the storage.
|no|POST|	/session/:sessionId/log	|Get the log for a given log type.
|no|GET|	/session/:sessionId/log/types	|Get available log types.
|no|GET|	/session/:sessionId/application_cache/status	|Get the status of the html5 application cache.

## Not ready features
Below features are not ready yet
- xpath locator
- Mouse Input support. WinUI functionality in [InputHelper.cs](https://github.com/microsoft/microsoft-ui-xaml/blob/master/test/testinfra/MUXTestInfra/Common/InputHelper.cs)
- Integration with appium
- ExecuteScript


## Download & Run YWinAppDriver
- Download and compile 
There are two ways to get the WinAppDriver.exe:
1. Clone this repo, then open the WinAppDriver.sln and build/run WinAppDriver project.
2. or Download it from https://github.com/licanhua/YWinAppDriver/releases

- Lauch WinAppDriver.exe
Please set your test endpoint to http://127.0.0.1:4723. Logs are in `Logs/WinAppDriver-{Date}.txt`.
If you run it from visual studio, there is no logs. If you want it, just remove `else` from below code 
```
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        loggerFactory.AddFile("Logs/WinAppDriver-{Date}.txt");
      }
```

If you want to use other port and url, please change these lines and rebuild the project

If you launch it outside of Visual studio, run `WinAppDriver.exe --urls http://127.0.0.1:4723`

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
