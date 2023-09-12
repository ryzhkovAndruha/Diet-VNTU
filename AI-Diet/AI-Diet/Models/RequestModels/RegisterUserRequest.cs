namespace AI_Diet.Models.RequestModels
{
    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
