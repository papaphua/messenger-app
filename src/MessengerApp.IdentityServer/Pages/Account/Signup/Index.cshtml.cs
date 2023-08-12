using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using MessengerApp.IdentityServer.Models;
using MessengerApp.IdentityServer.Pages.Account.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MessengerApp.IdentityServer.Pages.Account.Signup;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IEventService _events;
    private readonly IIdentityProviderStore _identityProviderStore;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _events = events;
    }

    [BindProperty] public InputModel Input { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "signup")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);

                return Redirect(Input.ReturnUrl);
            }

            // since we don't have a valid context, then we just go back to the home page
            return Redirect("~/");
        }

        if (Input.Email != null && await _userManager.FindByEmailAsync(Input.Email) != null)
            ModelState.AddModelError("Input.Email", SignupOptions.EmailAlreadyTakenErrorMessage);
        
        if (Input.Username != null && await _userManager.FindByNameAsync(Input.Username) != null)
            ModelState.AddModelError("Input.Username", SignupOptions.UsernameAlreadyTakenErrorMessage);
        
        if (ModelState.IsValid)
        {
            var createResult = await _userManager.CreateAsync(new ApplicationUser
            {
                Email = Input.Email,
                UserName = Input.Username
            }, Input.Password);

            if (createResult.Succeeded)
            {
                // var user = await _userManager.FindByNameAsync(Input.Username);
                //
                // await _userManager.AddClaimsAsync(user, new Claim[]
                // {
                //     new(JwtClaimTypes.Id, user.Id.ToString()),
                //     new(JwtClaimTypes.PreferredUserName, user.UserName)
                // });
                
                if (context != null)
                {
                    if (context.IsNativeClient())
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage(Input.ReturnUrl);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(Input.ReturnUrl);
                }
                
                // request for a local page
                if (Url.IsLocalUrl(Input.ReturnUrl))
                    return Redirect(Input.ReturnUrl);
                if (string.IsNullOrEmpty(Input.ReturnUrl))
                    return Redirect("~/");
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(Input.Username, "invalid credentials",
                clientId: context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
        }
        
        return Page();
    }
}