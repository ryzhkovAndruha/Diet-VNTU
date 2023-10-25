namespace AI_Diet.Models.ResponseModels
{
    public class DeleteUserResponse
    {
        public bool IsDeleted { get; set; }
        public List<string> Errors { get; set; }

        public DeleteUserResponse(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
