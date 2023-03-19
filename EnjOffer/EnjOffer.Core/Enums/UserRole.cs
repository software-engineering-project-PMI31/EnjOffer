using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace EnjOffer.Core.Enums
{
    [NpgsqlTypes.PgName("user_role")]
    public enum UserRole
    {
        SuperAdmin,
        Admin,
        User,
        Guest
    }
}
