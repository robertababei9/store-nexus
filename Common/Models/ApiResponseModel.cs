namespace Common.Models
{
    public class ApiResponseModel<T>
    {
        public ApiResponseModel()
        {
            Success = true;
            Errors = new List<string>();
        }


        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
