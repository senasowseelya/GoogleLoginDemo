using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GoogleLoginDemo.Services
{
	public class AuthenticationService
	{
		private readonly IConfigurationRoot _configuration;
		readonly RsaSecurityKey _key;
		public AuthenticationService()
        {
			_configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			RSA rsa = RSA.Create();
			rsa.ImportRSAPrivateKey(Convert.FromBase64String(_configuration.GetSection("SigningKey").Value),out _);
			_key = new RsaSecurityKey(rsa);
		}
        public string Authenticate(string idToken)
		{
			GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
			settings.Audience = new List<string>() { "708313847097-qqhkk449k8ut39q0uf0290rhvgm4cthh.apps.googleusercontent.com" };
			GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(idToken, settings).Result;
			return CreateAuthToken(payload);
		}

		public string CreateAuthToken(GoogleJsonWebSignature.Payload payload)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = "AuthService",
				Audience = "clientApp",
				Expires = DateTime.UtcNow.AddDays(1),
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Sid, payload.Email)
				}),
				SigningCredentials = new SigningCredentials(_key,SecurityAlgorithms.RsaSha256)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);

		}

	}
}