namespace SupperHeroAPI_Dotnet8.DTO
{
    public class ApiResponse<T> 
    {
        public int stautsCode {  get; set; }
        public string message {  get; set; }
        public T? data { get; set; }
    }
}
