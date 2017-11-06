﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Xunit;

namespace IntegrationTests.AccountControllerTests
{
    public class RegisterNewAccountTests : AccountControllerTestsBase
    {
        [Fact]
        public async Task RegisterNewUserSuccessfulTest()
        {
            GivenValues();

            await WhenRegister();

            await ThenSuccessfulRegistered();
        }

        string userName;
        string email;
        string password;
        string retypePassword;

        HttpResponseMessage response;

        void GivenValues()
        {
            email = "test@test.email";
            userName = "testUserName";
            password = "ASde34";
            retypePassword = password;
        }

        async Task WhenRegister()
        {
            var formData = new Dictionary<string, string>()
            {
                {"__RequestVerificationToken", await GetAntiForgeryToken("Register")},
                {"Email", email},
                {"UserName", userName},
                {"Password", password},
                {"RetypePassword", retypePassword}
            };

            response = await CookieClient.PostAsync("Account/Register", formData);
        }

        async Task ThenSuccessfulRegistered()
        {
            //Throws Exception if not success
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Registering user successful", content);

            using (var connection = Startup.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM AspNetUsers";
                    var dbReader = await command.ExecuteReaderAsync();

                    //The new account is in the secound row. In the first row is the admin account
                    Assert.True(dbReader.Read());
                    Assert.True(dbReader.Read());

                    Assert.Equal(userName, dbReader["UserName"]);
                    Assert.Equal(email, dbReader["Email"]);
                    Assert.False(string.IsNullOrEmpty((string)dbReader["PasswordHash"]));
                }

                connection.Close();
            }
        }
    }
}
