using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace SecuroteckWebApplication.Models
{
    public class User
    {
        #region Task2
        // TODO: Create a User Class for use with Entity Framework
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        [Key]
        public string ApiKeyString { get; set; }
        public string UserName { get; set; }
        public User() { }
        #endregion
    }

    #region Task11?
    // TODO: You may find it useful to add code here for Log
    #endregion

    public class UserDatabaseAccess
    {
        #region Task3 
        // TODO: Make methods which allow us to read from/write to the database 
        public void startDB()
        {
            Guid ApiKey = Guid.NewGuid();
            string newApiKeyString = ApiKey.ToString();
            using (var ctx = new UserContext())
            {
                User user = new User()
                {
                    ApiKeyString = newApiKeyString,
                    UserName = "userTest"
                };

                ctx.Users.Add(user);
                ctx.SaveChanges();
                /*
                User checkUser = ctx.Users.Find(newApiKeyString);

                if (checkUser == null)
                {
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                }
                else
                {

                }
                */
            }
        }

        public string writeNewUserToDB(string userName)
        {
            Guid ApiKey = Guid.NewGuid();
            string newApiKeyString = ApiKey.ToString();
            using (var ctx = new UserContext())
            {
                User user = new User()
                {
                    ApiKeyString = newApiKeyString,
                    UserName = userName
                };
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
            return newApiKeyString;

            #endregion
        }

        public bool checkUserViaKeyBool(string inApiKey)
        {
            bool result = false;

            using (var ctx = new UserContext())
            {
                if (ctx.Users.Find(inApiKey) != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;

        }

        public bool checkUserViaNameBool(string name)
        {
            bool result = false;

            using (var ctx = new UserContext())
            {
                var nameSearch = ctx.Users
                     .Where(b => b.UserName == name)
                     .FirstOrDefault();
                try
                {
                    if (nameSearch.UserName == name)
                    {
                        result = true;
                    }
                }
                catch
                {
                    return result;
                }

            }
            return result;
        }

        public bool checkUserViaKeyAndNameBool(string[] user)
        {
            bool result = false;

            string inUserName = user[0];
            string inApiKey = user[1];

            using (var ctx = new UserContext())
            {
                if (ctx.Users.Find(inApiKey) != null)
                {
                    var nameSearch = ctx.Users
                     .Where(b => b.UserName == inUserName)
                     .FirstOrDefault();
                    try
                    {
                        if (nameSearch.UserName == inUserName)
                        {
                            result = true;
                        }
                    }
                    catch
                    {
                        return result;
                    }
                }
                return result;
            }
        }

        public User checkUserViaKeyObj(string ApiKey)
        {
            string inApiKey = ApiKey;
            User user = null;
            using (var ctx = new UserContext())
            {
                if (ctx.Users.Find(ApiKey) != null)
                {
                    user = ctx.Users.Find(ApiKey);
                }
                else
                {
                    //result = false;
                }
            }
            return user;
        }

        public bool deleteUser(string ApiKey)
        {
            bool result = false;
            //User deleteUser = checkUserViaKeyObj(ApiKey);
            using (var ctx = new UserContext())
            {
                var deleteUser = ctx.Users.Find(ApiKey);
                ctx.Users.Remove(deleteUser);
                ctx.SaveChanges();
                result = true;
            }

            return result;
        }
    }
}