using FitnessTracker.Common.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Research.SEAL;
using FitnessTracker.Common.Utils;

namespace FitnessTrackerClient
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("SEAL LAB");
            Console.WriteLine("Setting up encryption...\n");

            // Add Initialization code here

            // Add keys code here

            while (true)
            {
                PrintMenu();
                var option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        await SendPrime();
                        break;
                    case 2:
                        await GetAnswer();
                        break;
                }
            }
        }

        static async Task SendPrime()
        {
            Console.WriteLine("Enter the the number u want to compute: ");
            var newPrime = Convert.ToInt64(Console.ReadLine());

            if (newPrime < 0)
            {
                Console.WriteLine("The number u want to check must be > 0.");
                return;
            }

            var PrimeString = newPrime.ToString();
            var metricsRequest = new PrimeItem
            {
                Prime = PrimeString
            };
            await FitnessTrackerClient.AddNewPrime(metricsRequest);
        }

        private static async Task GetAnswer()
        {
            var answer = await FitnessTrackerClient.GetAnswer();

            PrintAnswer(answer);
        }

        private static void PrintAnswer(EncAnswerItem answer)
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine("********* Factors *********");
            //Console.WriteLine($"Factor1:{answer.Factor1}");
            //Console.WriteLine($"Factor2:{answer.Factor2}");
            Console.WriteLine(string.Empty);

            var context = SEALUtils.GetContext();
            var ciphertextPrime = SEALUtils.BuildCiphertextFromBase64String(answer.Prime, context);
            var ciphertextFactor1 = SEALUtils.BuildCiphertextFromBase64String(answer.Factor1, context);
            var ciphertextFactor2 = SEALUtils.BuildCiphertextFromBase64String(answer.Factor2, context);
            var publicKey = SEALUtils.BuildPublicKeyFromBase64String(answer.PublicKey, context);
            var secretKey = SEALUtils.BuildSecretKeyFromBase64String(answer.SecretKey, context);

            Ciphertext temp = new Ciphertext();
            Evaluator _evaluator = new Evaluator(context);
            Encryptor encryptor = new Encryptor(context, publicKey);
            _evaluator.Multiply(ciphertextFactor1, ciphertextFactor2, temp);
            var tempstring = SEALUtils.CiphertextToBase64String(temp);
            if (tempstring.Equals(answer.Prime))
            {
                Console.WriteLine("the answer is right!");

            }
            else
            {
                var plain = new Plaintext();
                Console.WriteLine("the answer is wrong");
                Decryptor _decryptor = new Decryptor(context, secretKey);
                _decryptor.Decrypt(ciphertextPrime, plain);
                PrintAnswer(plain.ToString());
                encryptor.Encrypt(plain, temp);
                if (!SEALUtils.CiphertextToBase64String(temp).Equals(SEALUtils.CiphertextToBase64String(ciphertextPrime)))
                {
                    Console.WriteLine(SEALUtils.CiphertextToBase64String(ciphertextFactor2).Substring(0,100));
                    Console.WriteLine(SEALUtils.CiphertextToBase64String(ciphertextFactor1).Substring(0, 100));
                }
                Console.WriteLine(_decryptor.InvariantNoiseBudget(temp));  
                _decryptor.Decrypt(temp, plain);
                PrintAnswer(plain.ToString());
                //_decryptor.Decrypt(ciphertextFactor1, plain);
                //PrintAnswer(plain.ToString());
                //_decryptor.Decrypt(ciphertextFactor2, plain);
                //PrintAnswer(plain.ToString());
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("********* Menu (enter the option number and press enter) *********");
            Console.WriteLine("1. Add ur prime");
            Console.WriteLine("2. Get factors");
            Console.Write("Option: ");
        }

        private static void PrintAnswer(string str)
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine("********* ANSWER *********");
            Console.WriteLine($"Total runs: {int.Parse(str, System.Globalization.NumberStyles.HexNumber)}");
            Console.WriteLine(string.Empty);
        }
    }
}
