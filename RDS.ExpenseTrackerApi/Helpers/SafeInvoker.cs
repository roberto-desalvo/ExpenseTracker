using System.Diagnostics;

namespace RDS.ExpenseTrackerApi.Helpers
{
    public static class SafeInvoker
    {
        public static bool SafeInvoke(Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch(Exception ex)  
            {
                Trace.WriteLine(ex, ex.Message);
                return false;
            }
        }
    }
}
