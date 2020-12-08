using FitnessTracker.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using FitnessTracker.Common.Utils;

namespace FitnessTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimeController : Controller
    {
        // UNDONE 添加相关加密属性

        private long _prime = 0;
        private long[] _answer = { 0, 0 };

        private SEALContext _sealContext;
        private KeyGenerator _keyGenerator;

        private readonly Encryptor _encryptor;

        private string _base64Prime = "0", _base64Factor1 = "0", _base64Factor2 = "0", _publicKey = "0", _secretKey = "0";


        public PrimeController()
        {
            _sealContext = SEALUtils.GetContext();
            _keyGenerator = new KeyGenerator(_sealContext);
            _encryptor = new Encryptor(_sealContext, _keyGenerator.PublicKey);
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
            for (long i = 2; i < _prime / 2; i++)
            {
                if (_prime % i == 0)
                {
                    _answer[0] = i;
                    _answer[1] = _prime / i;
                    break;
                }
            }

            // TODO 此处添加对_answer和 _prime加密的功能
            var plainprime = new Plaintext($"{_prime.ToString("X")}");
            Console.WriteLine(_prime);
            var ciphertextprime = new Ciphertext();
            _encryptor.Encrypt(plainprime, ciphertextprime);
            var base64Prime = SEALUtils.CiphertextToBase64String(ciphertextprime);

            var plainfactor1 = new Plaintext($"{_answer[0].ToString("X")}");
            var ciphertextfactor1 = new Ciphertext();
            _encryptor.Encrypt(plainfactor1, ciphertextfactor1);
            var base64Factor1 = SEALUtils.CiphertextToBase64String(ciphertextfactor1);

            var plainfactor2 = new Plaintext($"{_answer[1].ToString("X")}");
            var ciphertextfactor2 = new Ciphertext();
            _encryptor.Encrypt(plainfactor2, ciphertextfactor2);
            var base64Factor2 = SEALUtils.CiphertextToBase64String(ciphertextfactor2);

            var publicKey = SEALUtils.PublicKeyToBase64String(_keyGenerator.PublicKey);
            var secretKey = SEALUtils.SecretKeyToBase64String(_keyGenerator.SecretKey);

            _base64Prime = base64Prime;
            _base64Factor1 = base64Factor1;
            _base64Factor2 = base64Factor2;
            _publicKey = publicKey;
            _secretKey = secretKey;
            

            return Ok();
        }

        [HttpGet]
        [Route("")]
        public ActionResult<EncAnswerItem> GetEncAnswer()
        {
            var answer = new EncAnswerItem
            {
                Prime = _base64Prime.ToString(),
                Factor1 = _base64Factor1.ToString(),
                Factor2 = _base64Factor2.ToString(),
                PublicKey = _publicKey.ToString(),
                SecretKey = _secretKey.ToString()
            };

            return answer;
        }

        [HttpGet]
        [Route("1")]
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
