using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Settings
{
   public interface IJwtTokenGenerator
   {
       Task<string> GenerateJwtTokenString(string username, string password);
   }
}
