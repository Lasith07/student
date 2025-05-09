namespace DemoAPI.DTOs.Responses
{
    public class MessageDTO
    {
        public string Token { get; set; }

        public MessageDTO(string token)
        {
            Token = token;
        }
    }
}
