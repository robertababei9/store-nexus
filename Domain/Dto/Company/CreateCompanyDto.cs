

namespace Domain.Dto.CompanyDto
{
    public class CreateCompanyDto
    {
        public string Name { get; set; }
        public int NoEmployees { get; set; }
        public string Type { get; set; }
        public string? Address { get; set; }
        public string? WebsiteUrl{ get; set; }
        public string? Contact { get; set; }
    }
}
