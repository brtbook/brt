using System;
using brt.Models.Registry;
using brt.Microservices.Registry.Interface;
using brt.Microservices.Common.Store;

namespace brt.Microservices.Registry.Service
{
    public class RegistryService : IRegistry
    {
        private readonly IDbase _dbase;

        public RegistryService(string docdburi, string docdbkey, string database, string collection)
        {
            _dbase = new Dbase(docdburi, docdbkey);
            _dbase.Connect(database, collection);
        }

        public Profile Create(Profile profile)
        {
            try
            {
                if (profile.IsValid())
                {
                    _dbase.Insert(profile);
                }
                else
                {
                    throw new Exception(Errors.ERR_PROFILE_MODEL_INVALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profile;
        }

        public void Delete(string id)
        {
            try
            {
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }
        }

        public Profiles GetAll()
        {
            Profiles profiles = new Profiles();

            try
            {
                var profileList = _dbase.SelectAll<Profile>();
                foreach (var u in profileList)
                {
                    profiles.list.Add(u);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_NOT_FOUND, err);
            }

            return profiles;
        }

        public Profiles GetAllByType(int type)
        {
            Profiles profiles = new Profiles();

            try
            {
                var query = $"SELECT * FROM ProfileModel p WHERE p.type={type}";
                var profileList = _dbase.SelectByQuery<Profile>(query);
                if (profileList != null)
                {
                    foreach (var p in profileList)
                    {
                        profiles.list.Add(p);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profiles;
        }

        public Profiles GetAllByCompany(string company)
        {
            Profiles profiles = new Profiles();

            try
            {
                var query = "SELECT * FROM ProfileModel p WHERE p.companyname='" + company + "'";
                var profileList = _dbase.SelectByQuery<Profile>(query);
                if (profileList != null)
                {
                    foreach (var p in profileList)
                    {
                        profiles.list.Add(p);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profiles;
        }

        public Profile GetById(string id)
        {
            Profile profile = null;

            try
            {
                profile = _dbase.SelectById<Profile>(id); ;
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profile;
        }

        public Profile GetByAuthId(string authid)
        {
            Profile profile = null;

            try
            {
                var query = $"SELECT * FROM ProfileModel p WHERE p.authid='{authid}'";
                var profileList = _dbase.SelectByQuery<Profile>(query);

                if (profileList != null)
                {
                    if (profileList.Count > 0)
                        profile = profileList[0];
                }
                else
                {
                    throw new Exception(Errors.ERR_PROFILE_NOT_FOUND);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profile;
        }

        public Profile GetByName(string firstname, string lastname)
        {
            Profile profile = null;

            try
            {
                var query = "SELECT * FROM ProfileModel p WHERE p.firstname='" + firstname + "' and p.lastname='" + lastname + "'";
                var profileList = _dbase.SelectByQuery<Profile>(query);

                if (profileList != null)
                {
                    if (profileList.Count > 0)
                        profile = profileList[0];
                }
                else
                {
                    throw new Exception(Errors.ERR_PROFILE_NOT_FOUND);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profile;
        }

        public Profiles GetAllByState(string state)
        {
            Profiles profiles = new Profiles();

            try
            {
                var query = "SELECT * FROM ProfileModel p where p.address.state='" + state + "'";
                var profileList = _dbase.SelectByQuery<Profile>(query);
                if (profileList != null)
                {
                    foreach (var p in profileList)
                    {
                        profiles.list.Add(p);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profiles;
        }

        public Profile Update(Profile profile)
        {
            try
            {
                if (profile.IsValid())
                    this._dbase.Update(profile);
                else
                {
                    throw new Exception(Errors.ERR_PROFILE_MODEL_INVALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_PROFILE_BADREQUEST, err);
            }

            return profile;
        }
    }
}
