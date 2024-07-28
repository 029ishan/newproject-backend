using System.ComponentModel.DataAnnotations;

public class SignupDetail
{
    public int Id { get; set; }

   
    public string Name { get; set; }

  
    public string Email { get; set; }

   
    public string Password { get; set; }

   
   
    public string ConfirmPassword { get; set; }

    public bool Role { get; set; } // Add this if it is required.
}
