// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.IdentityServer.Pages.Account.Signup;

public class InputModel
{
    [Required] public string Email { get; set; }
    
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
    
    [Required]
    [DisplayName("Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }

    public string Button { get; set; }
}