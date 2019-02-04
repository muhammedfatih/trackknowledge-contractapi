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

namespace ContractAPI.Controllers
{
    [BearerAuthentication]
    public class ContractsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Contracts.ToList());
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (ContractDBContext db = new ContractDBContext())
            {
                return db.Contracts.Find(id) == null ? Request.CreateResponse(HttpStatusCode.OK, new Contract()) : Request.CreateResponse(HttpStatusCode.Created, db.Contracts.Find(id));
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