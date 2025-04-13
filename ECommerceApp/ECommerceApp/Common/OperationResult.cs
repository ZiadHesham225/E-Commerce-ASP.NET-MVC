namespace ECommerceApp.Helpers
{
    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string ErrorMessage { get; set; }
        public bool IsAdmin { get; set; }

        public static OperationResult Success(bool isAdmin = false)
        {
            return new OperationResult { Succeeded = true, IsAdmin = isAdmin };
        }

        public static OperationResult Failure(string errorMessage)
        {
            return new OperationResult { Succeeded = false, ErrorMessage = errorMessage };
        }

        public static OperationResult Failure(List<string> errors)
        {
            return new OperationResult { Succeeded = false, Errors = errors };
        }
    }
}
