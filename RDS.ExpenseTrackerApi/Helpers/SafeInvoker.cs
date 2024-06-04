namespace RDS.ExpenseTrackerApi.Helpers
{
    public static class SafeInvoker
    {
        public static bool SafeInvoke(Action action)
        {
            try
            {
                action();
                return true;
            }catch (Exception ex)
            {
                // log error
                return false;
            }
        }
    }
}
