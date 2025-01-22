using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Account
{
    public class TokenResponse
    {
        [Required]
        [MinLength(1)]
        public string token { get; set; }

        public TokenResponse(string token)
        {
            this.token = token;
        }
    }
}
