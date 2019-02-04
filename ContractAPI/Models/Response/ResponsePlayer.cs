using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContractAPI.Models.Response
{
    public class ResponsePlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NationalityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ResponseNationality Nationality { get; set; }
        public ResponsePlayer()
        {
            Nationality = new ResponseNationality();
        }
    }
}