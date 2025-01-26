namespace InventoryManagement.Configurations
{
    public class JwtSettings
    {
        public required string Secret { get; set; }
        public int ExpirationDays { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
