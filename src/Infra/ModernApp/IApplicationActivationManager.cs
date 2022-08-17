using System;
using System.Runtime.InteropServices;

namespace WinAppDriver.Infra.ModernApp
{
  [ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  interface IApplicationActivationManager
  {
    // Activates the specified immersive application for the "Launch" contract, passing the provided arguments
    // string into the application.  Callers can obtain the process Id of the application instance fulfilling this contract.
    IntPtr ActivateApplication([In] string appUserModelId, [In] string arguments, [In] ActivateOptions options,
      [Out] out int processId);

    IntPtr ActivateForFile([In] string appUserModelId,
      [In] [MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] /*IShellItemArray* */
      IShellItemArray itemArray, [In] string verb, [Out] out int processId);

    IntPtr ActivateForProtocol([In] string appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray,
      [Out] out int processId);
  }
}