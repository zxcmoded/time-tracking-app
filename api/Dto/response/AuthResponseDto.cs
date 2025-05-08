namespace tracking_app.Dto
{
    public class AuthResponseDto : ResponseDto
    {
        public AuthResponseDto(bool success, string message) {
            Success = success;
            Message = message;
        }
    }
}