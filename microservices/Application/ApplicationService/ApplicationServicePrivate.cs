using System;
using brt.Microservices.Application.Interface;
using brt.Microservices.Common.Store;
using brt.Models.Application;

namespace brt.Microservices.Application.Service
{
    public class ApplicationServicePrivate : IApplicationPrivate
    {
        private IDbase _dbase;

        public ApplicationServicePrivate(string docdburi, string docdbkey, string database, string collection)
        {
            this._dbase = new Dbase(docdburi, docdbkey);
            this._dbase.Connect(database, collection);
        }

        public Configuration Create(Configuration configuration)
        {
            try
            {
                if (configuration.isValid())
                    this._dbase.Insert(configuration);
                else
                {
                    throw new Exception(Errors.ERR_APPLICATION_MODEL_NOT_VALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_APPLICATION_BAD_REQUEST, err);
            }

            return configuration;
        }

        public Configuration Update(Configuration configuration)
        {
            try
            {
                if (configuration.isValid())
                    this._dbase.Update(configuration);
                else
                {
                    throw new Exception(Errors.ERR_APPLICATION_MODEL_NOT_VALID);                    
                }
            }
            catch (Exception err)
            {
                throw new Exception(string.Format(Errors.ERR_APPLICATION_MODEL_NOT_UPDATED, configuration.appname), err);
            }

            return configuration;
        }

        public void Delete(string id)
        {
            try
            {
                this._dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_APPLICATION_MODEL_NOT_DELETED, err);
            }
        }

        public Applications GetAll()
        {
            Applications applications = new Applications();

            try
            {
                var configList = this._dbase.SelectAll<Configuration>();
                foreach(var c in configList)
                {
                    applications.list.Add(c);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_APPLICATION_BAD_REQUEST, err);
            }

            return applications;
        }

        public Configuration GetById(string id)
        {
            Configuration configuration = null;

            try
            {
                configuration = this._dbase.SelectById<Configuration>(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_APPLICATION_BAD_REQUEST, err);
            }

            return configuration;
        }

        public Configuration GetByAppName(string appname)
        {
            Configuration configuration = null;

            try
            {
                var query = $"select * from Configuration c where c.appnanme='{appname}'";
                var configList = this._dbase.SelectByQuery<Configuration>(query);
                if (configList != null)
                    configuration = configList[0];
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_APPLICATION_BAD_REQUEST, err);
            }

            return configuration;
        }

    }
}