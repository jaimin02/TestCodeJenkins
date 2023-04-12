using System;
using System.Security.Principal;

public class UserImpersonation
{
    const int LOGON32_LOGON_INTERACTIVE = 2;
    const int LOGON32_LOGON_NETWORK = 3;
    const int LOGON32_LOGON_BATCH = 4;
    const int LOGON32_LOGON_SERVICE = 5;
    const int LOGON32_LOGON_UNLOCK = 7;
    const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
    const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
    const int LOGON32_PROVIDER_DEFAULT = 0;
    const int LOGON32_PROVIDER_WINNT35 = 1;
    const int LOGON32_PROVIDER_WINNT40 = 2;
    const int LOGON32_PROVIDER_WINNT50 = 3;

    private WindowsImpersonationContext impersonationContext;

    [System.Runtime.InteropServices.DllImport("advapi32.dll")]
    static extern int LogonUserA(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

    [System.Runtime.InteropServices.DllImport("advapi32.dll")]
    static extern int DuplicateToken(IntPtr ExistingTokenHandle, int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

    [System.Runtime.InteropServices.DllImport("advapi32.dll")]
    static extern long RevertToSelf();
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    static extern long CloseHandle(IntPtr handle);

    public bool impersonateUser(string userName, string domain, string password)
    {
        return impersonateValidUser(userName, domain, password);
    }

    public void undoimpersonateUser()
    {
        undoImpersonation();
    }

    private bool impersonateValidUser(string userName, string domain, string password)
    {
        WindowsIdentity tempWindowsIdentity;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;
        Boolean impersonateValidUser = false;

        if (RevertToSelf() > 0)
        {
            if (LogonUserA(userName, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_WINNT50, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                        impersonateValidUser = true;
                }
            }
        }
        if (!tokenDuplicate.Equals(IntPtr.Zero))
            CloseHandle(tokenDuplicate);
        if (!token.Equals(IntPtr.Zero))
            CloseHandle(token);

        return impersonateValidUser;
    }

    private void undoImpersonation()
    {
        impersonationContext.Undo();
    }

    public bool impersonateLocalUser(string userName, string domain, string password)
    {
        return impersonateLocalValidUser(userName, domain, password);
    }

    public void undoimpersonateLocalUser()
    {
        undoImpersonation();
    }

    private bool impersonateLocalValidUser(string userName, string domain, string password)
    {
        WindowsIdentity tempWindowsIdentity;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;
        Boolean impersonateLocalValidUser = false;

        if (RevertToSelf() > 0)
        {
            if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                        impersonateLocalValidUser = true;
                }
            }
        }
        if (!tokenDuplicate.Equals(IntPtr.Zero))
            CloseHandle(tokenDuplicate);
        if (!token.Equals(IntPtr.Zero))
            CloseHandle(token);

        return impersonateLocalValidUser;
    }

    private void undoLocalImpersonation()
    {
        impersonationContext.Undo();
    }
}
