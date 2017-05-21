using System;
using brt.Microservices.Application.Interface;
using brt.Microservices.Common.Store;
using brt.Models.Application;

namespace brt.Microservices.Application.Service
{
    public class ApplicationServicePublic : IApplicationPublic
    {
        private IDbase _dbase;

        public ApplicationServicePublic(string docdburi, string docdbkey, string database, string collection)
        {
            this._dbase = new Dbase(docdburi, docdbkey);
            this._dbase.Connect(database, collection);
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