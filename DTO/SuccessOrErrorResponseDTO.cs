namespace performance_test.DTO
{
    public class SuccessOrErrorResponseDTO<T>
    {
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
