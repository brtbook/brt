using System;
using brt.Microservices.Customer.Interface;
using brt.Models.Customer;
using brt.Microservices.Common.Store;

namespace brt.Microservices.Customer.Service
{
    public class CustomerService : ICustomer
    {
        private readonly IDbase _dbase;

        public CustomerService(string docdburi, string docdbkey, string database, string collection)
        {
            _dbase = new Dbase(docdburi, docdbkey);
            _dbase.Connect(database, collection);
        }

        public Organization Create(Organization organization)
        {
            try
            {
                if (organization.IsValid())
                {
                    _dbase.Insert(organization);
                }
                else
                {
                    throw new Exception(Errors.ERR_Customer_INVALIDMODEL);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_Customer_BADREQUEST, err);
            }

            return organization;
        }

        public void Delete(string id)
        {
            try
            {
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_Customer_BADREQUEST, err);
            }
        }

        public Customers GetAll()
        {
            var customers = new Customers();

            try
            {
                var orgList = this._dbase.SelectAll<Organization>();
                foreach (var o in orgList)
                {
                    customers.list.Add(o);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_Customer_BADREQUEST, err);
            }

            return customers;
        }

        public Organization GetById(string id)
        {
            Organization organization = null;

            try
            {
                var query = $"select * from Customer o where o.id='{id}'";
                var orgList = _dbase.SelectByQuery<Organization>(query);
                if (orgList[0] != null)
                {
                    organization = orgList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_Customer_BADREQUEST, err);
            }

            return organization;
        }

        public Organization GetByName(string name)
        {
            Organization organization = null;

            try
            {
                var query = $"select * from Customer o where o.name='{name}'";
                var orgList = _dbase.SelectByQuery<Organization>(query);
                if (orgList[0] != null)
                {
                    organization = orgList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_Customer_BADREQUEST, err);
            }

            return organization;
        }

        public Organization Update(Organization orginization)
        {
            try
            {
                if (orginization.IsValid())
                    this._dbase.Update(orginization);
                else
                {
                    throw new Exception(Errors.ERR_Customer_INVALIDMODEL);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_Customer_BADREQUEST, err);
            }

            return orginization;
        }
    }
}
