using System;
using brt.Microservices.Reference.Interface;
using brt.Models.Reference;
using brt.Microservices.Common.Store;

namespace brt.Microservices.Reference.Service
{
    public class ReferenceServicePrivate : IReferencePrivate
    {
        private readonly IDbase _dbase;

        public ReferenceServicePrivate(string docdburi, string docdbkey, string database, string collection)
        {
            _dbase = new Dbase(docdburi, docdbkey);
            _dbase.Connect(database, collection);
        }

        public Entity Create(Entity entity)
        {
            try
            {
                if (entity.isValid())
                    _dbase.Insert(entity);
                else
                {
                    throw new Exception(Errors.ERR_REF_ENTITY_NOT_VALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entity;
        }

        public void Delete(string id)
        {
            try
            {
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }
        }

        public Entities GetAll()
        {
            Entities entities = new Entities();

            try
            {
                 var entityList = _dbase.SelectAll<Entity>();
                foreach (Entity e in entityList)
                {
                    entities.list.Add(e);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entities;
        }

        public Entity Update(Entity entity)
        {
            try
            {
                if (entity.isValid())
                {
                    _dbase.Update(entity);
                }
                else
                {
                    throw new Exception(Errors.ERR_REF_ENTITY_NOT_VALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_REF_BADREQUEST, err);
            }

            return entity;
        }
    }
}
