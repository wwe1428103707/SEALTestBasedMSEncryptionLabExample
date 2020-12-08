using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FitnessTracker.Common.Models;
using FitnessTracker.Common.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private List<double> _distances = new List<double>();
        private List<double> _times = new List<double>();
        private double _prime = 0;
        private double[] _answer = new double[2];

        public MetricsController()
        {
            // Initialize context

            // Initialize key generator and encryptor

            // Initialize evaluator
        }

        [HttpGet]
        [Route("keys")]
        public ActionResult<KeysModel> GetKeys()
        {
            return null;
        }


        [HttpPost]
        [Route("")]
        public ActionResult ComputePrime([FromBody] PrimeItem request)
        {
            var prime = request.Prime;

            _prime = Convert.ToInt64(prime);
            for (int i = 2; i < _prime/2; i++)
            {
                if (_prime % i == 0)
                {
                    _answer[0] = i;
                    _answer[1] = _prime / i;
                    break;
                }
            }


            return Ok();
        }

        [HttpGet]
        [Route("")]
        public ActionResult<AnswerItem> GetAnswer()
        {
            var answer = new AnswerItem
            {
                Factor1 = _answer[0].ToString(),
                Factor2 = _answer[1].ToString()
            };

            return answer;
        }
    }
}