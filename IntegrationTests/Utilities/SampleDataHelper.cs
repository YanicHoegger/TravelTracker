using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IntegrationTests.Utilities
{
    public static class SampleDataHelper
    {
        public static void CreateAdmin(DatabaseFacade database, string userName, string email, string password)
        {
            var hashedPassword = HashPassword(password);

            database.ExecuteSqlCommand("Insert Sql query", userName, email, hashedPassword); //TODO
        }

        public static void CreateAdmin(DatabaseFacade database)
        {
            var userName = "Admin";
            var email = "Admin@Test.com";
            var password = "123Asd";

            CreateAdmin(database, userName, email, password);
        }

		public static string HashPassword(string password)
		{
			byte[] salt;
			byte[] buffer2;
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
			{
				salt = bytes.Salt;
				buffer2 = bytes.GetBytes(0x20);
			}
			byte[] dst = new byte[0x31];
			Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
			Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
			return Convert.ToBase64String(dst);
		}
    }
}
