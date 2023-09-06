namespace AI_Diet.Authorization.Models
{
    public class AddUserResult
    {
        public bool IsSuccess { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
