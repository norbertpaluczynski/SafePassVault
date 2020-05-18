using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace SafePassVault.Core.Models
{
    public class Service : BaseNotifyPropertyModel, IDataErrorInfo
    {
        private Guid _id;
        private string _name;
        private string _url;
        private string _login;
        private string _password;
        private string _description;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private DateTime? _passwordExpirationDate;

        public Service()
        {
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
            _passwordExpirationDate = null;
        }

        public Service(Service service)
        {
            _name = service.Name;
            _login = service.Login;
            _password = service.Password;
            _description = service.Description;
            _url = service.Url;
            _createdAt = service.CreatedAt;
            _updatedAt = service.UpdatedAt;
            _passwordExpirationDate = service._passwordExpirationDate;
        }


        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        [JsonProperty()]
        public string Name 
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Url 
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged();
            }
        }

        public string Login 
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string Password 
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Description 
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged();
            }
        }

        public DateTime? PasswordExpirationDate
        {
            get => _passwordExpirationDate;
            set
            {
                _passwordExpirationDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsPasswordExpired => _passwordExpirationDate < DateTime.Now;

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case "Name":
                        if(string.IsNullOrEmpty(Name))
                        {
                            return "Name is required!";
                        }
                        break;

                    case "Login":
                        if (string.IsNullOrEmpty(Login))
                        {
                            return "Login is required!";
                        }
                        break;

                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                        {
                            return "Password is required!";
                        }
                        break;
                    default: break;
                }

                return null;
            }
        }
    }
}
