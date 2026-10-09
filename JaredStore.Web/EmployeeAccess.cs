namespace JaredStore.Web
{
    public static class EmployeeAccess
    {
        // DEMO ONLY: Replace with secure authentication
        // and credential storage in a production application.
        public static bool IsValid(string? username, string? password)
        {
            return username == "employee1"
                && password == "password";
        }
    }
}
