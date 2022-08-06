using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinAppDriver.Infra.ModernApp
{
    [ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")] //Application Activation Manager
    class ApplicationActivationManager : IApplicationActivationManager
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime) /*, PreserveSig*/]
        public extern IntPtr ActivateApplication([In] string appUserModelId, [In] string arguments,
            [In] ActivateOptions options, [Out] out int processId);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public extern IntPtr ActivateForFile([In] string appUserModelId,
            [In] [MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] /*IShellItemArray* */
            IShellItemArray itemArray, [In] string verb, [Out] out int processId);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public extern IntPtr ActivateForProtocol([In] string appUserModelId,
            [In] IntPtr /* IShellItemArray* */itemArray, [Out] out int processId);
    }
}