namespace AI_Diet.Models.ResponseModels
{
    public class RegisterUserResponseErrors: RegisterUserResponse
    {
        public IEnumerable<string> RegisterErrors { get; set; }
    }
}
