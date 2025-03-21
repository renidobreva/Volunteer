using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Можеш да добавиш допълнителни свойства тук
    public string FullName { get; set; }
}
