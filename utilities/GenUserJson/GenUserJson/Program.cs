using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using brt.Microservices.Common.Wire;
using brt.Models.Registry;
using Newtonsoft.Json;

namespace GenUserJson
{
    class Program
    {
        private static string _devSubKey;
        private static string _registryApi;

        static void Main(string[] args)
        {
            _devSubKey = ConfigurationManager.AppSettings["DevSubKey"];
            _registryApi = ConfigurationManager.AppSettings["RegistryAPI"];

            var profiles = GetAllCompanyProfiles("WigiTech");
            profiles.list.RemoveAt(0); // remove the company profile
            GenUserJson(profiles);

            profiles = GetAllCompanyProfiles("Tall Towers");
            GenUserJson(profiles);

            profiles = GetAllCompanyProfiles("The Complicated Badger");
            GenUserJson(profiles);
        }

        private static void GenUserJson(Profiles profiles)
        {
            foreach (var p in profiles.list)
            {
                var user = new User
                {
                    accountEnabled = true,
                    signInNames = new List<SignInName> { new SignInName() }
                };

                user.signInNames[0].type = "emailAddress";
                user.signInNames[0].value = p.social.email;
                user.creationType = "LocalAccount";
                user.displayName = p.firstname + " " + p.lastname;
                user.mailNickname = p.firstname + "." + p.lastname;

                user.passwordProfile = new PasswordProfile
                {
                    forceChangePasswordNextLogin = false,
                    password = "P@ssword!"
                };

                user.passwordPolicies = "DisablePasswordExpiration";
                user.city = p.address.city;
                user.country = p.address.country;
                user.facsimileTelephoneNumber = null;
                user.givenName = p.firstname;
                user.mail = null;
                user.mobile = p.social.phone;
                user.otherMails = new List<object>();
                user.postalCode = p.address.zip;
                user.preferredLanguage = null;
                user.state = p.address.state;
                user.streetAddress = p.address.address1;
                user.surname = p.lastname;
                user.telephoneNumber = p.social.phone;

                // save to JSON file
                var json = JsonConvert.SerializeObject(user);
                var path = Directory.GetCurrentDirectory();
                var filename = p.firstname + "." + p.lastname + ".json";
                using (var outputFile = new StreamWriter(path + @"\json\" + filename))
                {
                    outputFile.WriteLine(json);
                }
            }
        }

        private static Profiles GetAllCompanyProfiles(string company)
        {
            var uri = _registryApi + @"/profiles/company/" + company;

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profiles = ModelManager.JsonToModel<Profiles>(json);

            return profiles;
        }

    }

    public class SignInName
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class PasswordProfile
    {
        public string password { get; set; }
        public bool forceChangePasswordNextLogin { get; set; }
    }

    public class User
    {
        public bool accountEnabled { get; set; }
        public List<SignInName> signInNames { get; set; }
        public string creationType { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public PasswordProfile passwordProfile { get; set; }
        public string passwordPolicies { get; set; }
        public string city { get; set; }
        public object country { get; set; }
        public object facsimileTelephoneNumber { get; set; }
        public string givenName { get; set; }
        public object mail { get; set; }
        public object mobile { get; set; }
        public List<object> otherMails { get; set; }
        public string postalCode { get; set; }
        public object preferredLanguage { get; set; }
        public string state { get; set; }
        public object streetAddress { get; set; }
        public string surname { get; set; }
        public object telephoneNumber { get; set; }
    }
}
