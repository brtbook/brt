using System;
using brt.Microservices.Simulation.Interface;
using brt.Models.Simulation;
using brt.Microservices.Common.Store;

namespace brt.Microservices.Simulation.Service
{
    public class SimulationService : ISimulation
    {
        private IDbase _dbase;

        public SimulationService(string docdburi, string docdbkey, string database, string collection)
        {
            _dbase = new Dbase(docdburi, docdbkey);
            _dbase.Connect(database, collection);
        }

        public DataSet CreateDataSet(DataSet dataset)
        {
            try
            {
                _dbase.Insert(dataset);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return dataset;
        }

        public void DeleteDataSet(string id)
        {
            try
            {
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }
        }

        public DataSet GetDataSetById(string id)
        {
            DataSet dataset = null;

            try
            {
                var query = $"select * from DataSet d where d.id='{id}'";
                var datasetList = _dbase.SelectByQuery<DataSet>(query);
                if (datasetList.Count > 0)
                {
                    dataset = datasetList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return dataset;
        }

        public DataSet GetDataSetByName(string name)
        {
            DataSet dataset = null;

            try
            {
                var query = $"select * from DataSet d where d.name='{name}'";
                var datasetList = _dbase.SelectByQuery<DataSet>(query);
                if (datasetList.Count > 0)
                {
                    dataset = datasetList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return dataset;
        }

        public DataRow GetDataRowById(string id)
        {
            DataRow row = null;

            try
            {
                var query = $"select * from DataRow r where r.id='{id}'";
                var datarowList = _dbase.SelectByQuery<DataRow>(query);
                if (datarowList.Count > 0)
                {
                    row = datarowList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return row;
        }

        public DataSet UpdateDataSet(DataSet dataset)
        {
            try
            {
                this._dbase.Update(dataset);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return dataset;
        }

        public DataRow CreateDataRow(DataRow datarow)
        {
            try
            {
                _dbase.Insert(datarow);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return datarow;
        }

        public DataRow UpdateDataRow(DataRow datarow)
        {
            try
            {
                this._dbase.Update(datarow);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }

            return datarow;
        }

        public void DeleteDataRow(string id)
        {
            try
            {
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_SIMULATION_BADREQUEST, err);
            }
        }
    }
}
