using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ContractAPI.Models;
using DBContext;
using System.Net;
using System.Net.Http;
using FluentValidation.Results;
using AutoMapper;
using ContractAPI.Helpers;
using ContractAPI.Models.Response;
using RestSharp;
using System.Configuration;
using Newtonsoft.Json;

namespace ContractAPI.Controllers
{
    [BearerAuthentication]
    public class ContractsController : ApiController
    {

        public HttpResponseMessage Get()
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                List<ResponseContract> returnList = new List<ResponseContract>();
                foreach (var item in db.Contracts.ToList())
                {
                    ResponseContract itemToAdd = new ResponseContract();

                    var Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_PLAYER"]);
                    var uri = string.Format("/players/{0}", item.PlayerId);
                    var request = new RestRequest(uri, Method.GET);
                    request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
                    IRestResponse response = Client.Execute(request);
                    var responsePlayer = JsonConvert.DeserializeObject<ResponsePlayer>(response.Content);

                    Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_TEAM"]);
                    uri = string.Format("/teams/{0}", item.TeamId);
                    request = new RestRequest(uri, Method.GET);
                    request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
                    response = Client.Execute(request);
                    var responseTeam = JsonConvert.DeserializeObject<ResponseTeam>(response.Content);

                    itemToAdd = Mapper.Map<ResponseContract>(item);
                    itemToAdd.Player = responsePlayer;
                    itemToAdd.Team = responseTeam;
                    returnList.Add(itemToAdd);
                }
                return Request.CreateResponse(HttpStatusCode.OK, returnList);
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                ResponseContract returnItem = new ResponseContract();
                Contract item = db.Contracts.Find(id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, returnItem);
                }
                else
                {
                    var Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_PLAYER"]);
                    var uri = string.Format("/players/{0}", item.PlayerId);
                    var request = new RestRequest(uri, Method.GET);
                    request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
                    IRestResponse response = Client.Execute(request);
                    var responsePlayer = JsonConvert.DeserializeObject<ResponsePlayer>(response.Content);

                    Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_TEAM"]);
                    uri = string.Format("/teams/{0}", item.TeamId);
                    request = new RestRequest(uri, Method.GET);
                    request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
                    response = Client.Execute(request);
                    var responseTeam = JsonConvert.DeserializeObject<ResponseTeam>(response.Content);

                    returnItem = Mapper.Map<ResponseContract>(item);
                    returnItem.Player = responsePlayer;
                    returnItem.Team = responseTeam;
                    return Request.CreateResponse(HttpStatusCode.OK, returnItem);
                }
            }
        }

        public HttpResponseMessage GetLastByPlayerId(int playerId)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                var contractId=db.Contracts.Where(x => x.PlayerId == playerId).OrderByDescending(x => x.StartAt).Select(x => x.Id).FirstOrDefault();
                return Get(contractId);
            }
        }

        public HttpResponseMessage GetAllByPlayerId(int playerId)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                List<ResponseContract> returnList = new List<ResponseContract>();
                foreach (var item in db.Contracts.Where(x=>x.PlayerId==playerId).OrderByDescending(x=>x.StartAt).ToList())
                {
                    ResponseContract itemToAdd = new ResponseContract();

                    var Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_PLAYER"]);
                    var uri = string.Format("/players/{0}", item.PlayerId);
                    var request = new RestRequest(uri, Method.GET);
                    request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
                    IRestResponse response = Client.Execute(request);
                    var responsePlayer = JsonConvert.DeserializeObject<ResponsePlayer>(response.Content);

                    Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_TEAM"]);
                    uri = string.Format("/teams/{0}", item.TeamId);
                    request = new RestRequest(uri, Method.GET);
                    request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
                    response = Client.Execute(request);
                    var responseTeam = JsonConvert.DeserializeObject<ResponseTeam>(response.Content);

                    itemToAdd = Mapper.Map<ResponseContract>(item);
                    itemToAdd.Player = responsePlayer;
                    itemToAdd.Team = responseTeam;
                    returnList.Add(itemToAdd);
                }
                return Request.CreateResponse(HttpStatusCode.OK, returnList);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Contract request)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                ValidationContract validator = new ValidationContract();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    db.Contracts.Add(request);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, request);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors.Select(x => x.ErrorMessage).ToList());
                }
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Contract request)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                var record = db.Contracts.Find(id);
                if (record == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, record);
                }
                else
                {
                    if (request.PlayerId != 0) record.PlayerId = request.PlayerId;
                    if (request.TeamId != 0) record.TeamId = request.TeamId;
                    if (request.YearlySalary != 0) record.YearlySalary = request.YearlySalary;
                    if (request.TransferFee != 0) record.TransferFee = request.TransferFee;
                    if (request.StartAt != DateTime.MinValue) record.StartAt = request.StartAt;
                    if (request.EndAt != DateTime.MinValue) record.EndAt = request.EndAt;

                    ValidationContract validator = new ValidationContract();
                    ValidationResult result = validator.Validate(record);

                    if (result.IsValid)
                    {
                        record.UpdatedAt = DateTime.Now;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, record);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors.Select(x => x.ErrorMessage).ToList());
                    }
                }
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                var record = db.Contracts.Find(id);
                if (record == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, record);
                }
                else
                {
                    db.Contracts.Remove(record);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, record);
            }
        }
    }
}