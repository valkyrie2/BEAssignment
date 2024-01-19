using System.ComponentModel.DataAnnotations;

namespace BE.Model
{
    public class UserResponse
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        //...
    }
}
