using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MultiNotes.Windows.Services
{
    public interface IHavePassword
    {
        SecureString Password { get; }
    }

    class PasswordService
    {
        //Odbezpieczanie hasła
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
