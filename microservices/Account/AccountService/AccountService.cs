using System;
using brt.Models.Account;
using brt.Microservices.AccountAPI.Interface;
using brt.Microservices.Common.Store;

namespace brt.Microservices.AccountAPI.Service
{
    public class AccountService : IAccount
    {
        private readonly IDbase _dbase;

        public AccountService(string docdburi, string docdbkey, string database, string collection)
        {
            _dbase = new Dbase(docdburi, docdbkey);
            _dbase.Connect(database, collection);
        }

        public Subscription Create(Subscription subscription)
        {
            try
            {
                if (subscription.IsValid())
                {
                    _dbase.Insert(subscription);
                }
                else
                {
                    throw new Exception(Errors.ERR_ACCOUNT_INVALIDMODEL);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_ACCOUNT_BADREQUEST, err);
            }

            return subscription;
        }

        public void Delete(string id)
        {
            try
            {
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_ACCOUNT_BADREQUEST, err);
            }
        }

        public Accounts GetAll()
        {
            var accounts = new Accounts();

            try
            {
                var subList = this._dbase.SelectAll<Subscription>();
                foreach (var a in subList)
                {
                    accounts.list.Add(a);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_ACCOUNT_BADREQUEST, err);
            }

            return accounts;
        }

        public Subscription GetById(string id)
        {
            Subscription subscription = null;

            try
            {
                var query = $"select * from AccountModel a where a.id='{id}'";
                var subList = _dbase.SelectByQuery<Subscription>(query);
                if (subList[0] != null)
                {
                    subscription = subList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_ACCOUNT_BADREQUEST, err);
            }

            return subscription;
        }

        public Subscription GetByOrganizationId(string organizationid)
        {
            Subscription subscription = null;

            try
            {
                var query = $"select * from AccountModel a where a.organizationId='{organizationid}'";
                var subList = _dbase.SelectByQuery<Subscription>(query);
                if (subList[0] != null)
                {
                    subscription = subList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_ACCOUNT_BADREQUEST, err);
            }

            return subscription;
        }

        public Subscription Update(Subscription subscription)
        {
            try
            {
                if (subscription.IsValid())
                    this._dbase.Update(subscription);
                else
                {
                    throw new Exception(Errors.ERR_ACCOUNT_INVALIDMODEL);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_ACCOUNT_BADREQUEST, err);
            }

            return subscription;
        }
    }
}
