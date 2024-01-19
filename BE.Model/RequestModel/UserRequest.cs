using System.ComponentModel.DataAnnotations;

namespace BE.Model
{

    public class UserCreateRequest
    {
        public long Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        //...
    }

    public class UserUpdateRequest
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        //...
    }
}
