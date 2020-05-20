using System;
using System.Collections.Generic;
using System.Text;

namespace GpsServer.Teltonika.Server
{
    using System.Threading.Tasks;

    using global::UAC.EndPoints.Service.Base;
    using global::UAC.EndPoints.Service.Device.Enums;
    using global::UAC.EndPoints.Service.UserAccount.RequestsArg;

    public class UserAcc
    {
        private string token = "NO_TOKEN";

        private IUacService uac;

        public UserAcc(IUacService uacService)
        {
            this.uac = uacService;
        }

        public async Task<string> GetToken()
        {
            var isValidToken = false;
            try
            {
                isValidToken = await this.uac.UserSessionService.ValidateToken(token).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                isValidToken = false;
            }

            if (!isValidToken)
            {
                var login = await this.uac.UserAccountService.LoginAsync(new LoginUserArg { Username = "hessam", Password = "3592", Platform = PlatformType.Windows }).ConfigureAwait(false);
                token = login.Token;
            }
            return token;
        }




    }
}
