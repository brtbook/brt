using brt.Models.Simulation;

namespace brt.Microservices.Simulation.Interface
{
    public interface ISimulation
    {
        DataSet GetDataSetByName(string name);
        DataSet GetDataSetById(string id);
        DataRow GetDataRowById(string id);
        DataSet CreateDataSet(DataSet dataset);
        DataSet UpdateDataSet(DataSet dataset);
        DataRow CreateDataRow(DataRow datarow);
        DataRow UpdateDataRow(DataRow datarow);
        void DeleteDataSet(string id);
        void DeleteDataRow(string id);
    }
}
