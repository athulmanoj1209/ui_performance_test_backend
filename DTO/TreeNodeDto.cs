namespace performance_test.DTO
{
    public class TreeNodeDto
    {
        public NodeDataDto Data { get; set; } = new();
        public List<TreeNodeDto>? Children { get; set; }
    }
}
