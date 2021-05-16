using MyRecipes.Core.Mobile.Encryption;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Mobile.Account
{
    class User : ViewModelBase
    {
        private string mUsername;
        private string mEmail;
        private string mPasswordEncrypted;

        public string Username
        {
            get => mUsername;
            set
            {
                mUsername = value;
                InvokePropertyChanged();
            }
        }

        public string Email
        {
            get => mEmail;
            set
            {
                mEmail = value;
                //TODO: Add verification process if email changes
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Sets the unhashed password and returns the hashed password.
        /// </summary>
        [JsonIgnore]
        public string Password
        {
            get => mPasswordEncrypted;
            set
            {
                mPasswordEncrypted = Hasher.HashPassword(value, mUsername);
                InvokePropertyChanged();
            }
        }

        //For Json files only; Encrypted password gets further encrypted with salt
        public string PasswordSaved
        {
            get => Hasher.EncryptString(mPasswordEncrypted, Email);
            private set
            {
                mPasswordEncrypted = Hasher.DecryptString(value, Email);
                InvokePropertyChanged("Password");
            }
        }

        [JsonConstructor]
        public User(string name, string passwordSaved, string email)
        {
            mUsername = name;
            mEmail = email;
            PasswordSaved = passwordSaved;
        }

        /// <summary>
        /// Called on login
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        public User(string email, string password)
        {

        }

        /// <summary>
        /// Called on registering
        /// </summary>
        /// <param name="username">The username of the new user</param>
        /// <param name="email">The email of the new user</param>
        /// <param name="password">The password of the new user</param>
        /// <param name="isRegister">Leave as is</param>
        public User(string username, string email, string password, bool isRegister = true)
        {

        }
    }
}
