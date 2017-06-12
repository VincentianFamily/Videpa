using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Videpa.Identity.Logic
{
    public class JwtService
    {
        private const string PrivateKey = "%3cRSAKeyValue%3e%3cModulus%3erwS88beYNVHhdxOH4FpvTIIY1y3tc9FESx%2fsgux4e1OGabfP6wXV0vnt1uerBpDSFcXFUdj5A3LW8syjIIXTzB7YoqKOcZR1p39rI40hosbQ5hoOSd4hyDeMbx7eVQnENtCenCWgG8IeNouEc%2bzHD0Iw4ApoAP5a4zQ2sTFkp4ZKJPFL75DnmZLbpLqsZXwn%2bxyinZB6Nr8sFc8DK8fczpBItJQgB%2fL06E%2bQG3K0Gs71Fd9wjW6VxU4aud%2b3Mx1%2b7pppLsANGuc%2bmuv5RuPnACvWwGY3TkerMG7Vu9cSTUocDhviU5SKMN9dWUkXjI%2byJsyHPnjgyDs%2bf8IlgBG8uw%3d%3d%3c%2fModulus%3e%3cExponent%3eAQAB%3c%2fExponent%3e%3cP%3e3Nj%2fjXG3wqCLYMcGYtyHaA7v0Jj61K9%2fuQrl82n3jByakImG8eSWMO997F2DRcvq2bZGHGFubC2h44kVjpreBVkgBWtli9sjOgn6XqnfYOYvvy1wQrvyPs8N0fpL0dM0cb844n6teuZKTpf5%2fgJ91uIA47tLybSaV4UFZbutbCM%3d%3c%2fP%3e%3cQ%3eyuBQeVoJxSrsndoKy4dmEWlL1slE%2fB8a36xdkcGwUbboD3j4GiIT6E5X9l8ir52haOOuyDS7LCMh0j%2b5cmwPeqtXsFAxc7AhcBAp7kZj5oj94aREzeePZpzroUVH%2bgrqbAXO0O0SYETtJVIqQsaVHz4RUwNPETyoxryWvz%2boiok%3d%3c%2fQ%3e%3cDP%3ehVtrhWfDxPzY7QmBa7Uq9sT%2bPwIBhOiswXo5PPEiEDM9ikXhfLPBla8oZNBa8oyeNfUBBSY4XY54kUM4BzKxGp2k3mDporgi32sjCoKdZo%2fZCSJRBZskPpUdDUyM%2bBkHNCB6IxrHes0Q1cCjrqSmhFlYnCt%2fKj%2bqa2rhxXq8z%2b0%3d%3c%2fDP%3e%3cDQ%3erqziPQnb%2fapOnj69Kq5CoF0j0iPrRJTXg5wZ2ESHNmQgfCnwnDeZIlxJ%2bjPBXql%2b7xuSQoi8W21IUolRd4Y48aqTTphNa8lLkPwpziFHJJfw2BWitBJa8sPLUUigh1ZcrnN1tYdZWhAeixKqlmH6xTKgofPoYPP0cWCw%2fqulxtk%3d%3c%2fDQ%3e%3cInverseQ%3eTk7s%2bDYxSkOmMoNGiQS%2fzNJJZeUwvX%2bpIozVXU9ohrLsSkwXyUw%2fQiEv7MU5mXIj5mrCGEk5QX9bpHhU1yTxZxlBqupI9vYDOUNlekMzwI9GTKm75mEm5mR3VBcSKjjund08VBK1kYCUdzbmQA7OqbxTYpGVjpD7dcEctgGjvgE%3d%3c%2fInverseQ%3e%3cD%3eAEZqoZqfQ54wU4aVVRXvTNh7f4MT93WhvNh8%2f6uteMwlzu66Be2Ja353EDpm%2b2io293mtH05zvaUPedybi%2bnqI932bGVtbn9AVli6rhjECVt2J673QPbW7wynU4YHVwZkdh8F1S7zq9casuHVsNNgMmi2AdMKIw9ylOfrmGcfeLoenDTcf32cZV7OXE%2fwHLf9VazLmGvEnnUPXbMqOVj%2bjMIS9dao7HgnoD4c8iwmAi5zRYOSEI0f8Vj6TkUgDdHGEkbrReobMxdnWIy9CPmfEzZ%2btkObYBILAnfMMDOUIhp3aWJeVXNPJ4J8OHG8KG2jfAnBjxsF%2b1l55MZtxuwcQ%3d%3d%3c%2fD%3e%3c%2fRSAKeyValue%3e";
        private const string PublicKey = "%3cRSAKeyValue%3e%3cModulus%3erwS88beYNVHhdxOH4FpvTIIY1y3tc9FESx%2fsgux4e1OGabfP6wXV0vnt1uerBpDSFcXFUdj5A3LW8syjIIXTzB7YoqKOcZR1p39rI40hosbQ5hoOSd4hyDeMbx7eVQnENtCenCWgG8IeNouEc%2bzHD0Iw4ApoAP5a4zQ2sTFkp4ZKJPFL75DnmZLbpLqsZXwn%2bxyinZB6Nr8sFc8DK8fczpBItJQgB%2fL06E%2bQG3K0Gs71Fd9wjW6VxU4aud%2b3Mx1%2b7pppLsANGuc%2bmuv5RuPnACvWwGY3TkerMG7Vu9cSTUocDhviU5SKMN9dWUkXjI%2byJsyHPnjgyDs%2bf8IlgBG8uw%3d%3d%3c%2fModulus%3e%3cExponent%3eAQAB%3c%2fExponent%3e%3c%2fRSAKeyValue%3e";
        private const string TokenIssuer = "http://www.bluegarden.dk";
        private const int JwtDurationInMinutes = 120;

        public string GenerateToken(ClaimsIdentity identity)
        {
            var privateXml = Uri.UnescapeDataString(PrivateKey);
            var publicAndPrivate = new RSACryptoServiceProvider();
            
            publicAndPrivate.FromXmlString(privateXml);

            var now = DateTime.UtcNow;
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                TokenIssuerName = TokenIssuer,
                AppliesToAddress = TokenIssuer,
                Lifetime = new Lifetime(now, now.AddMinutes(JwtDurationInMinutes)),
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(publicAndPrivate), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public ClaimsPrincipal VerifyJwt(string jwt)
        {
            var publicXml = Uri.UnescapeDataString(PublicKey);

            var publicOnly = new RSACryptoServiceProvider();
            publicOnly.FromXmlString(publicXml);

            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningToken = new RsaSecurityToken(publicOnly),
                ValidIssuer = TokenIssuer,
                ValidAudience = TokenIssuer
            };

            SecurityToken securityToken;
            var principal = new JwtSecurityTokenHandler().ValidateToken(jwt, validationParameters, out securityToken);
            return principal;
        }
    }
}
