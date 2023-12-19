using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class RolePermissions : BaseEntity<Guid>
    {
        public string Name { get; set; }

        #region Dashboard
        public bool ViewDashboard { get; set; }
        public bool EditDashboard { get; set; }
        #endregion

        #region Stores
        public bool ViewStore { get; set; }
        public bool EditStore { get; set; }
        public bool CreateStore { get; set; }
        public bool DeleteStore { get; set; }
        #endregion

        #region Invoices
        public bool ViewInvoice { get; set; }
        public bool CreateInvoice { get; set; }
        #endregion

        #region Users
        public bool ViewUser { get; set; }
        public bool EditUser { get; set; }
        public bool CreateUser { get; set; }
        public bool DeleteUser { get; set; }
        #endregion

        #region Settings
        public bool Settings { get; set; }

        #endregion

        [JsonIgnore]
        public virtual Role Role { get; set; }    // one to many relationship


        public bool MapPermissions(Dictionary<string, bool> permissions)
        {
            bool result = true;
            foreach(var kvp in permissions)
            {
                // Check if the property exist in the RolePermissions class
                var property = typeof(RolePermissions).GetProperty(kvp.Key);
                if (property != null && property.CanWrite)
                {
                    // Update the property value
                    property.SetValue(this, kvp.Value);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
