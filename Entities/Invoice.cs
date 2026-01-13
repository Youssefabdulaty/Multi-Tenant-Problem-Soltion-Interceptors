using MinimalAPI.Interfaces;

namespace MinimalAPI.Entities
{
    public sealed class Invoice : IAuditableEntity, ISoftDelete
    {
        public Guid Id { get; set; }             // MUST be public
        public Guid TenantId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EUR";

        // Audit fields → MUST be public
        public DateTimeOffset CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Soft delete fields → MUST be public
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }

}
