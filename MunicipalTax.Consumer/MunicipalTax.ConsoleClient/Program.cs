using MunicipalTax.Common.DataModels;
using MunicipalTax.Entity;
using System;
using System.Configuration;
using System.Net.Http;

namespace MunicipalTax.ConsoleClient
{
    class Program
    {
        private static string producerEndpoint;
       
        static void Main(string[] args)
        {
            producerEndpoint = ConfigurationManager.AppSettings["ProducerEndpoint"];

            Console.WriteLine("*** Welcome to Municipality Tax Manager ***");

            Console.WriteLine("Please enter the municipality name for which tax has to be determined.\n");
            var municipalityName = Console.ReadLine();
            Console.WriteLine("Please enter the date for which tax has to be determined.\n");
            var dateString = Console.ReadLine();

            if(!string.IsNullOrEmpty(municipalityName) && !string.IsNullOrEmpty(dateString))
            {
                Console.WriteLine("Invalid data entered.\n");
            }
            else
            {
                var taxRate = GetMunicipalityTax(municipalityName, dateString);
                Console.WriteLine($"Tax applied in municpality: {municipalityName} for the date: {dateString} is: {taxRate}");
            }          
            
        }

        /// <summary>
        /// Get municipality tax.
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static double GetMunicipalityTax(string municipalityName, string dateString)
        {
            double taxRate = 0.0;

            try
            {
                using (var client = new HttpClient())
                {
                    var uriBuilder = new UriBuilder(producerEndpoint)
                    {
                        Query = $"municipalityName={municipalityName}&dateString={dateString}"
                    };

                    var response = client.GetAsync(uriBuilder.Uri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseModel = response.Content.ReadAsAsync<ResponseModel>().Result;
                        if (responseModel != null && responseModel.Success)
                        {
                            var municipalityTax = responseModel.Payload as MunicipalityTax;
                            taxRate = municipalityTax.Tax;
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Some error occured.");
            }
            return taxRate;
        }
    }
}
