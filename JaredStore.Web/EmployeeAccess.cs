namespace JaredStore.Web
{
    public static class EmployeeAccess
    {
        // DEMO ONLY: For demo purposes, I have hardcoded a single employee username and password. In a real application, you would want to implement a proper authentication system with hashed passwords and user management.
        public static bool IsValid(string? username, string? password)
        {
            return username == "employee1"
                && password == "password";
        }
    }
}
