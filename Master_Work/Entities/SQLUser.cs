using Master_Work.Interfaces.EntityInterface;
using System.ComponentModel.DataAnnotations;

namespace Master_Work.Entities
{
    public class SQLUser
    {
        public class SQLRegister : IEntity<long>
        {
            public long Id { get; set; }
            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, MinLength(6)]
            public string Password { get; set; }

            [Required]
            [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
