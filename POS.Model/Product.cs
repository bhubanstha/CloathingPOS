namespace POS.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public Category CategoryId { get; set; }
    }
}