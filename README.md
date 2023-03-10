# Yet Another WinAppDriver (YWinAppDriver)

[Microsoft WinAppDriver](https://github.com/Microsoft/WinAppDriver) is the official application  to support Selenium-like UI Test Automation on Windows Applications. This service supports testing Universal Windows Platform (UWP), Windows Forms (WinForms), Windows Presentation Foundation (WPF), and Classic Windows (Win32) apps on Windows 10 PCs.

This repo is an open source asp.net core implementation of WinAppDriver and it's compatible with Microsoft's WinAppDriver. Most of the ideas are coming from the test infrastructure of [WinUI](https://github.com/microsoft/microsoft-ui-xaml), [Microsoft.Windows.Apps.Test](https://github.com/Microsoft/Microsoft.Windows.Apps.Test), and [WinAppDriver document](https://github.com/microsoft/WinAppDriver/tree/master/Docs). I combined them and come up with the open source implementation. 

I name this project YWinAppDriver(yet another WinAppDriver).

[![Build Status](https://dev.azure.com/licanhua/YWinAppDriver/_apis/build/status/licanhua.YWinAppDriver?branchName=main)](https://dev.azure.com/licanhua/YWinAppDriver/_build/latest?definitionId=2&branchName=main)

## Project Status: 0.3.x
Use .Net 6 instead of .net core 3.1

## Project Status: 0.2.x
   Most of the functionalities are ready and you should be able to switch from WinAppDriver to YWinAppDriver without any(or With little) change. [SessionController.cs](https://github.com/licanhua/YWinAppDriver/blob/main/src/WinAppDriver/Controllers/SessionController.cs) defines all the endpoints it supported. 
   
For the XPath syntax, refer to https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)

## Download & Run YWinAppDriver
- Download and compile

There are two ways to get the WinAppDriver.exe:
1. Clone this repo, then open the WinAppDriver.sln and build WinAppDriver project.
2. or Download it from https://github.com/licanhua/YWinAppDriver/releases

- Lauch WinAppDriver.exe
Generally speaking, WinAppDriver user would have two settings: http://127.0.0.1:4723 or http://127.0.0.1:4723/wd/hub.
Note: [dotnet-core runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1) is required, please install 3.1 or late it if you have problem to run WinAppDriver.exe

By default, YWinAppDriver is http://127.0.0.1:4723. You can change the port number and basepath easily:
1. CLI

run `WinAppDriver.exe --urls http://127.0.0.1:4723 --basepath /wd/hub`


A complete command line:
```
WinAppDriver.exe --urls http://127.0.0.1:4723 --basepath /wd/hub --logpath logs
```

2. From Visual Studio

There are two [settings](docs/images/LaunchFromVS.png) are ready for you. `IIS Express /wd/hub` is http://127.0.0.1:4723/wd/hub

3. Using appsettings.json

```
"Urls": "http://127.0.0.1:4723",
"Basepath": "/wd/hub"

```

- Build and run the CalcatorTest in [examples](https://github.com/licanhua/YWinAppDriver/tree/main/examples/CalculatorTest)
Please run the test, please make sure Calculator is in Standard mode.

## nodejs YWinAppDriver/WinAppDriver examples:
If you are authoring the test case with Jest, Jasmine or any other JavaScript framework, you can switch between YWinAppDriver and WinAppDriver very easily.

[wdio + YWinAppDriver/WinAppDriver](https://github.com/licanhua/wdio-winappdriver-example)

[selenium + YWinAppDriver/WinAppDriver](https://github.com/react-native-windows/selenium-appium/tree/master/example)


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

## API Supported
Below is the table to match with [selenium json wire protocol](https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol#command-summary).
Because YWinAppDriver is for desktop application other than browser, `no` below means it's not supported.
`maybe` means it's possible to support it, but I didn't see any value to support it. 

| Dev Status | HTTP Method |	Path	| Summary
|---|---|---|---|
| completed |GET	|/status    |	Query the server's current status.
| completed |POST   |/session   |	Create a new session.
| completed |GET    |/sessions  |	Returns a list of the currently active sessions.
| completed |GET	|/session/:sessionId	|   Retrieve the capabilities of the specified session.
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
|complete|GET|	/session/:sessionId/screenshot	|Take a screenshot of the current page
|complete|GET|	/session/:sessionId/element/:elementId/screenshot	|Take a screenshot of the element
|no|GET|	/session/:sessionId/ime/available_engines	|List all available engines on the machine.
|no|GET|	/session/:sessionId/ime/active_engine	|Get the name of the active IME engine.
|no|GET|	/session/:sessionId/ime/activated	|Indicates whether IME input is active at the moment (not if it's available.
|no|POST|	/session/:sessionId/ime/deactivate	|De-activates the currently-active IME engine.
|no|POST|	/session/:sessionId/ime/activate	|Make an engines that is available (appears on the listreturned by getAvailableEngines) active.
|no|POST|	/session/:sessionId/frame	|Change focus to another frame on the page.
|no|POST|	/session/:sessionId/frame/parent	|Change focus to the parent context.
|completed|POST|	/session/:sessionId/window	|Change focus to another window.
|completed|DELETE|	/session/:sessionId/window	|Close the current window.
|completed|POST|	/session/:sessionId/window/:windowHandle/size	|Change the size of the specified window.
|completed|GET|	/session/:sessionId/window/:windowHandle/size	|Get the size of the specified window.
|completed|POST|	/session/:sessionId/window/:windowHandle/position	|Change the position of the specified window.
|completed|GET|	/session/:sessionId/window/:windowHandle/position	|Get the position of the specified window.
|completed|POST|	/session/:sessionId/window/:windowHandle/maximize	|Maximize the specified window if not already maximized.
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
|completed|POST|	/session/:sessionId/moveto	|Move the mouse by an offset of the specificed element.
|completed|POST|	/session/:sessionId/click	|Click any mouse button (at the coordinates set by the last moveto command).
|completed|POST|	/session/:sessionId/buttondown	|Click and hold the left mouse button (at the coordinates set by the last moveto command).
|completed|POST|	/session/:sessionId/buttonup	|Releases the mouse button previously held (where the mouse is currently at).
|completed|POST|	/session/:sessionId/doubleclick	|Double-clicks at the current mouse coordinates (set by moveto).
|completed|POST|	/session/:sessionId/touch/click	|Single tap on the touch enabled device.
|completed|POST|	/session/:sessionId/touch/down	|Finger down on the screen.
|completed|POST|	/session/:sessionId/touch/up	|Finger up on the screen.
|completed|POST|	session/:sessionId/touch/move	|Finger move on the screen.
|in progress|POST|	session/:sessionId/touch/scroll	|Scroll on the touch screen using finger based motion events.
|in progress|POST|	session/:sessionId/touch/scroll	|Scroll on the touch screen using finger based motion events.
|completed|POST|	session/:sessionId/touch/doubleclick	|Double tap on the touch screen using finger motion events.
|completed|POST|	session/:sessionId/touch/longclick	|Long press on the touch screen using finger motion events.
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

## Supported Locators to Find UI Elements

Windows Application Driver supports various locators to find UI element in the application session. The table below shows all supported locator strategies with their corresponding UI element attributes shown in **inspect.exe**.

| Client API                   	| Locator Strategy 	| Matched Attribute in inspect.exe       	| Example      	|
|------------------------------	|------------------	|----------------------------------------	|--------------	|
| FindElementByAccessibilityId 	| accessibility id 	| AutomationId                           	| AppNameTitle 	|
| FindElementByClassName       	| class name       	| ClassName                              	| TextBlock    	|
| FindElementById              	| id               	| RuntimeId (decimal)                    	| 42.333896.3.1	|
| FindElementByName            	| name             	| Name                                   	| Calculator   	|
| FindElementByTagName         	| tag name         	| LocalizedControlType (upper camel case)	| Text         	|
| FindElementByXPath           	| xpath            	| Any                                    	| //Button[0]  	|

## Supported Capabilities

Below are the capabilities that can be used to create Windows Application Driver session.

| Capabilities       	| Descriptions                                          	| Example                                               	|
|--------------------	|-------------------------------------------------------	|-------------------------------------------------------	|
| app                	| Application identifier or executable full path        	| Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge   	|
| appArguments       	| Application launch arguments                          	| https://github.com/Microsoft/WinAppDriver             	|
| attachToTopLevelWindowClassName  	| app should be "Root", Existing application top level window to attach to. if you are using WinAppDriver, please use appTopLevelWindow    	| `0xB822E2`                                            	|
| appWorkingDir      	| Application working directory (Classic apps only)     	| `C:\Temp`                                             	|
| forceMatchAppTitle	| If app is launched, but have problem to match it, YWinAppDriver do the last try to match with the application title	| Calculator                                               	|
| forceMatchClassName	| If app is launched, but have problem to match it, YWinAppDriver do the last try to match with the class name	| Chrome_WidgetWin_1                                               	|
| clickWithInvoke   	| Use Inovke other than click to get better performance  	| true/false   	|

## YWinAppDriver addressed some WinAppDriver 1.2 issues and fixed them

### WinAppDriver has problem to start c:\windows\system32\calc.exe, PostMan, or Notepad++ [issue 1372](https://github.com/microsoft/WinAppDriver/issues/1372)
In YWinAppDriver, you can workaround the problem with the capabilites like below.
```
"forceMatchAppTitle": "Calculator"
"forceMatchClassName":"Notepad++"
```

### Appium desktop can't show all controls WinAppDriver provided
 [Appium Desktop](https://github.com/appium/appium-desktop) is a great tool to inspect the app's elements and it's very easy to learn and use it.
 Currently WinAppDriver can't show all elements. I don't know if it's the issue of WinAppDriver or Appium, but YWinAppDriver doesn't have this problem.

 ## YWinAppDriver Known issue
 ### The Click is slow than WinAppDriver
 You can set capabilities {clickWithInvoke: true}, it will be very fast. The down side is it will not close the flyout like Menu Windows until you click somewhere else.
