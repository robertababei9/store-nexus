

namespace Application
{
    // The place where frontend-app ROUTES and backend must be in sync
    // Whenever a new route it's added in the front-end this will need to be updated as well
    // Not hard but this will create a new dependency ...
    // The reason is that it will be needed to know what routes to render on the frontend based
    // on the User role permissions
    //
    // Only the private routes matter for the moment
    //
    public static class Routes
    {
        public static string Dashboard = "/dashboard";

        public static string Stores = "/stores";
        public static string StoresEdit = "/stores";
        public static string StoresCreate = "/stores";

        public static string Invoices = "/stores";
        public static string InvoicesCreate = "/stores";
        public static string InvoicesView = "/stores";

        public static string Users = "/stores";
        public static string EditUser = "/stores";
        public static string AddUser = "/stores";

        public static string CreateCompany = "/stores";
        public static string Settings = "/stores";
    }
}
