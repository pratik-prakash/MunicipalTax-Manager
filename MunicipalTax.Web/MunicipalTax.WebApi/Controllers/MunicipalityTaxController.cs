using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalTax.Common;
using MunicipalTax.Common.Services;
using MunicipalTax.Entity;

namespace MunicipalTax.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipalityTaxController : ControllerBase
    {
        readonly IMunicipalityTaxService _municipalityTaxService;

        public MunicipalityTaxController(IMunicipalityTaxService municipalityTaxService)
        {
            _municipalityTaxService = municipalityTaxService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <param name="dateString"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMunicipalityTax(string municipalityName, string dateString)
        {
            DateTime scheduledDate = Utility.GetDateFromString(dateString);

            if (!string.IsNullOrEmpty(municipalityName) && scheduledDate != DateTime.MinValue)
            {
                return Ok(_municipalityTaxService.GetMunicipalityTax(municipalityName, scheduledDate));
            }
            else
            {
                return BadRequest(Utility.CreateErrorResponse("Invalid parameters."));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [Route("GetMTax")]
        [HttpGet]
        public ActionResult GetMunicipalityTax(string municipalityName, DateTime date)
        {
            if (!string.IsNullOrEmpty(municipalityName) && date != DateTime.MinValue)
            {
                return Ok(_municipalityTaxService.GetMunicipalityTax(municipalityName, date));
            }
            else
            {
                return BadRequest(Utility.CreateErrorResponse("Invalid parameters."));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        [Route("AddMTax")]
        [HttpPost]
        public ActionResult AddMunicipalityTaxRecord([FromBody] MunicipalityTax municipalityTaxRecord)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model supplied.");

            if (!string.IsNullOrEmpty(municipalityTaxRecord.MunicipalityName)
               && Utility.ValidatePeriod(municipalityTaxRecord.TaxType, municipalityTaxRecord.StartDate, municipalityTaxRecord.EndDate))
            {
                return Ok(_municipalityTaxService.AddMunicipalityTax(municipalityTaxRecord));
            }
            else
            {
                return BadRequest(Utility.CreateErrorResponse("Invalid parameters."));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [Route("ImportMTaxDataFromFile")]
        [HttpPost]
        public ActionResult ImportMunicipalityTaxDataFromFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && Path.GetExtension(filePath).ToUpper() == ".CSV")
            {
                return Ok(_municipalityTaxService.ImportBulkMunicipalityTaxRecords(filePath));
            }
            else
            {
                return BadRequest(Utility.CreateErrorResponse($"Invalid parameters or file extension. Expected '.csv' file extension, Received: '{Path.GetExtension(filePath)}'"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult UpdateMunicipality([FromBody] MunicipalityTax municipalityTaxRecord)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data model supplied.");

            if (!string.IsNullOrEmpty(municipalityTaxRecord.MunicipalityName))
            {
                return Ok(_municipalityTaxService.UpdateMunicipalityTax(municipalityTaxRecord));
            }
            else
            {
                return BadRequest(Utility.CreateErrorResponse("Invalid parameters."));
            }

        }
    }
}
