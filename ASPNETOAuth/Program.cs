using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ASPNETOAuth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // TODO: Download from URL regularly--don't hard-code/embed!
        var jsonWebKeySet = new JsonWebKeySet("{" +
            "  \"keys\" : [" +
            "    {" +
            "      \"alg\" : \"RS256\"," +
            "      \"e\" : \"AQAB\"," +
            "      \"kid\" : \"Z6WCOmSkyd758XuV2mv0RoRn2Y4\"," +
            "      \"kty\" : \"RSA\"," +
            "      \"n\" : \"w69Bqz-Xep3AE8_wwBACj-PeRgZdaJ1rbQvjA4RTF5YqbeBJUeXKelC0PPQx0D1ZjaBdD-XrOAiDVSAk73JD7nmDuBb0d7Cs4scPKrLaO1PEpHPRxifgNdiMWb8MDnvsAMQnlvXg_IcnueS3-met_ApsmICGWR66_orETa-q9GgaXiWIWjgUyLpYmV70y37daceJIszb2keFKIYu2n6sw2Vf8SohAVNPm4liAP7joUo8WgIu0Mstv5xoI0v6UPrlDMUI1P7NheRqclEvDtmu-J311tkkT5c2Hw08kGoZybSo0CDvAdhTqMU2ZOlQRM-35gqTqBQTe4p_5CL2klEiyw\"," +
            "      \"use\" : \"sig\"," +
            "      \"x5c\" : [" +
            "        \"MIICrzCCAZegAwIBAQIRAJAvFGQLhURNgYJPyUEFPH8wDQYJKoZIhvcNAQELBQAwEzERMA8GA1UEAxMIYWNtZS5jb20wHhcNMjIwOTI5MTczODM1WhcNMzIwOTI5MTczODM1WjATMREwDwYDVQQDEwhhY21lLmNvbTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMOvQas/l3qdwBPP8MAQAo/j3kYGXWida20L4wOEUxeWKm3gSVHlynpQtDz0MdA9WY2gXQ/l6zgIg1UgJO9yQ+55g7gW9HewrOLHDyqy2jtTxKRz0cYn4DXYjFm/DA577ADEJ5b14PyHJ7nkt/pnrfwKbJiAhlkeuv6KxE2vqvRoGl4liFo4FMi6WJle9Mt+3WnHiSLM29pHhSiGLtp+rMNlX/EqIQFTT5uJYgD+46FKPFoCLtDLLb+caCNL+lD65QzFCNT+zYXkanJRLw7Zrvid9dbZJE+XNh8NPJBqGcm0qNAg7wHYU6jFNmTpUETPt+YKk6gUE3uKf+Qi9pJRIssCAwEAATANBgkqhkiG9w0BAQsFAAOCAQEAEQgw0fMV1nQv6miVEbgZ4Q3imoUiQO8eVlR2pnNAaWDs6/jeNZFIFs3GNN1kYwKglK2Phs47K/R6+pQpeTnbRd42w2iTHXDY8wnuN/QKjrPh796MDjXYt7/QJ8NZLwrU8c9BD11zuEL+WuvKsyuxtdQTTZQ4RGDzIFaLIl9Wzly2zEXm9RUsrb5o32VEX9d6kn82q46rJt0fiQ+LENAMtsmXK4UkAodr++HAyci1ftF/Z3NrnkSOJoElI+Z6Q67DZ2gzLyIUXfYt/373ruoswr51ICVrOec4xKaQyz3NeXxBLt0+TA/dpeXkr8j5M7KjuFn59DPPOvTNQmSV1Nd5dw==\"" +
            "      ]," +
            "      \"x5t\" : \"Z6WCOmSkyd758XuV2mv0RoRn2Y4\"," +
            "      \"x5t#S256\" : \"tupdoLf86NSvWu9dUCVfmBwGMTohh3CiYxmeGrv_650\"" +
            "    }" +
            "  ]" +
            "}");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = "Cookies";
            options.ClientId = "ca0584cb-76b9-464c-ad9a-056ef952bd61";
            options.ClientSecret = "VSohrEwtb8dZZ-w6sRDzjxXQGlKJKY61jLmuMqskcf4";
            options.Authority = "http://localhost:9011/.well-known/openid-configuration/ffb8aec6-af38-4711-9ef3-64493e937436";
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = false;

            options.ResponseType = "code";

            options.Scope.Add("profile");
            options.Scope.Add("offline");
            options.SaveTokens = true;
        })
        ;
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // routing, authentication, and authorization must be called in
        // that particular order!

        app.UseRouting();

        // Added
        app.UseAuthentication();
        // END Added

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
