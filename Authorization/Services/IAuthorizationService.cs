﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermission(string role, string[] permissions);
    }
}
