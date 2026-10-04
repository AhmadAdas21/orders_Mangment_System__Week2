namespace Week2_Task_2.Dto
{
    public class error_response
    {
        public int statusCode { get; set; }

        public string message { get; set; } = string.Empty;

        public List<string>? errors { get; set; }
    }
}