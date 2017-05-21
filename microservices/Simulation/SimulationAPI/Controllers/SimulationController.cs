using brt.Microservices.Simulation.Service;
using brt.Models.Simulation;
using System.Configuration;
using System.Web.Http;

namespace SimulationAPI.Controllers
{
    public class SimulationController : ApiController
    {
        SimulationService _simulationService;

        public SimulationController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time


            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];


            _simulationService = new SimulationService(docdburi, docdbkey, database, collection);
        }

        [Route("simulation/datasets")]
        [RequireHttps]
        [HttpPost]
        public DataSet CreateDataSet(DataSet dataset)
        {
            return _simulationService.CreateDataSet(dataset);
        }

        [Route("simulation/datasets/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void DeleteDataSet(string id)
        {
            _simulationService.DeleteDataSet(id);
        }

        [Route("simulation/datasets/name/{name}")]
        [RequireHttps]
        [HttpGet]
        public DataSet GetDataSetByName(string name)
        {
            return _simulationService.GetDataSetByName(name);
        }

        [Route("simulation/datasets/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public DataSet GetDataSetById(string id)
        {
            return _simulationService.GetDataSetById(id);
        }

        [Route("simulation/datasets/rows")]
        [RequireHttps]
        [HttpPost]
        public DataRow CreateDataRow(DataRow datarow)
        {
            return _simulationService.CreateDataRow(datarow);
        }

        [Route("simulation/datasets/rows/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void DeleteDataRow(string id)
        {
            _simulationService.DeleteDataRow(id);
        }


        [Route("simulation/datasets/rows/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public DataRow GetDataRowById(string id)
        {
            return _simulationService.GetDataRowById(id);
        }

        [Route("simulation/datasets")]
        [RequireHttps]
        [HttpPut]
        public DataSet UpdateDataSet(DataSet dataset)
        {
            return _simulationService.UpdateDataSet(dataset);
        }

        [Route("simulation/datasets/rows")]
        [RequireHttps]
        [HttpPut]
        public DataRow UpdateDataRow(DataRow datarow)
        {
            return _simulationService.UpdateDataRow(datarow);
        }

    }
}
