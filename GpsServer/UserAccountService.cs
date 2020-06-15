// ***********************************************************************
// Assembly         : GpsServer
// Author           : U12178
// Created          : 06-15-2020
//
// Last Modified By : U12178
// Last Modified On : 05-20-2020
// ***********************************************************************
// <copyright file="UAC.cs" company="Golriz">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace GpsServer
{
    using System;
    using System.Threading.Tasks;

    using UAC.EndPoints.Service.Base;
    using UAC.EndPoints.Service.Device.Enums;
    using UAC.EndPoints.Service.UserAccount.RequestsArg;

    /// <summary>
    /// Class UserAccount.
    /// </summary>
    public class UserAccountService
    {
        /// <summary>
        /// The token
        /// </summary>
        private string token = "NO_TOKEN";

        /// <summary>
        /// The uac
        /// </summary>
        private readonly IUacService uac;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountService"/> class.
        /// </summary>
        /// <param name="uacService">The uac service.</param>
        public UserAccountService(IUacService uacService)
        {
            this.uac = uacService;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetToken()
        {
            var isValidToken = false;
            try
            {
                isValidToken = await this.uac.UserSessionService.ValidateToken(this.token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isValidToken = false;
            }

            if (!isValidToken)
            {
                var login = await this.uac.UserAccountService.LoginAsync(new LoginUserArg { Username = "hessam", Password = "3592", Platform = PlatformType.Windows }).ConfigureAwait(false);
                this.token = login.Token;
            }

            return this.token;
        }
    }
}
