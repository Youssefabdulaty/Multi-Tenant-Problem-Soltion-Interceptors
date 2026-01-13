namespace MinimalAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Product { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}