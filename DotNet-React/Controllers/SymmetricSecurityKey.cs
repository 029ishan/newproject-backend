using Microsoft.IdentityModel.Tokens;

namespace DotNet_React.Controllers
{
    internal class SigningCredentials
    {
        private byte[] key;
        private SymmetricSecurityKey symmetricSecurityKey;
        private string hmacSha256Signature;

        public SigningCredentials(Microsoft.IdentityModel.Tokens.SymmetricSecurityKey symmetricSecurityKey, byte[] key)
        {
            this.key = key;
        }

        public SigningCredentials(SymmetricSecurityKey symmetricSecurityKey, string hmacSha256Signature)
        {
            this.symmetricSecurityKey = symmetricSecurityKey;
            this.hmacSha256Signature = hmacSha256Signature;
        }

        public static implicit operator Microsoft.IdentityModel.Tokens.SigningCredentials(SigningCredentials v)
        {
            throw new NotImplementedException();
        }
    }
}