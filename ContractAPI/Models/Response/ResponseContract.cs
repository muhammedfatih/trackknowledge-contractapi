using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContractAPI.Models.Response
{
    public class ResponseContract
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public int YearlySalary { get; set; }
        public int TransferFee { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ResponsePlayer Player { get; set; }
        public ResponseTeam Team { get; set; }
        public ResponseContract()
        {
            Player = new ResponsePlayer();
            Team = new ResponseTeam();
        }
    }
}