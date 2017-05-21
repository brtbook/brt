using System;
using BlueMetl.Microservices.Reference.Interface;
using brt.Models.Reference;
using brt.Microservices.Common.Store;

namespace brt.Microservices.Reference.Service
{
    public class ReferenceServicePublic : IReferencePublic
    {
        private readonly IDbase _dbase;

        public ReferenceServicePublic(string docdburi, string docdbkey, string database, string collection)
        {
            _dbase = new Dbase(docdburi, docdbkey);
            _dbase.Connect(database, collection);
        }

        public Entities GetAllByDomain(string domain)
        {
            Entities entities = new Entities();

            try
            {
                var query = "SELECT * FROM Entity e WHERE e.domain='" + domain + "'";
                var entityList = _dbase.SelectByQuery<Entity>(query);
                if (entityList != null)
                {
                    foreach( var e in entityList)
                    {
                        entities.list.Add(e);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entities;
        }

        public Entities GetAllByLink(string link)
        {
            Entities entities = new Entities();

            try
            {
                var query = "SELECT * FROM Entity e WHERE e.link='" + link + "'";
                var entityList = _dbase.SelectByQuery<Entity>(query);
                if (entityList != null)
                {
                    foreach (var e in entityList)
                    {
                        entities.list.Add(e);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entities;
        }

        public Entity GetByCode(string code)
        {
            Entity entity = null;

            try
            {
                var query = "SELECT * FROM Entity e WHERE e.code='" + code + "'";
                var entityList = _dbase.SelectByQuery<Entity>(query);
                if (entityList.Count > 0)
                    entity = entityList[0];
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entity;
        }

        public Entities GetByCodeValue(string codevalue)
        {
            Entities entities = new Entities();

            try
            {
                var query = "SELECT * FROM Entity e WHERE e.codevalue='" + codevalue + "'";
                var entityList = _dbase.SelectByQuery<Entity>(query);
                if (entityList != null)
                {
                    foreach (var e in entityList)
                    {
                        entities.list.Add(e);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entities;
        }
    }
}
