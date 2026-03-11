namespace performance_test.DTO
{
    public class ToPrimeTreeDto
    {
        public string Key { get; set; }
        public string label { get; set; }
        public NodeDataDto data { get; set; }
        public string? icon { get; set; }
        public List<ToPrimeTreeDto>? children { get; set; }
    }
}
