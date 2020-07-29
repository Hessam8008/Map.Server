// ***********************************************************************
// Assembly         : Map.Server
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-29-2020
// ***********************************************************************
// <copyright file="UserAccountService.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.Server
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
        /// The User Account Service.
        /// </summary>
        private readonly IUacService uac;

        /// <summary>
        /// The token
        /// </summary>
        private string token = "NO_TOKEN";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountService" /> class.
        /// </summary>
        /// <param name="uacService">The User Account Service.</param>
        public UserAccountService(IUacService uacService)
        {
            this.uac = uacService;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <returns>Task of string.</returns>
        public async Task<string> GetToken()
        {
            bool isValidToken;
            try
            {
                isValidToken = await this.uac.UserSessionService.ValidateToken(this.token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isValidToken = false;
            }

            if (isValidToken)
            {
                return this.token;
            }

            var login = await this.uac.UserAccountService.LoginAsync(new LoginUserArg { Username = "hessam", Password = "3592", Platform = PlatformType.Windows }).ConfigureAwait(false);
            this.token = login.Token;

            return this.token;
        }
    }
}
