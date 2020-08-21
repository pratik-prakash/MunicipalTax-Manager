using MunicipalTax.Common.DataModels;
using MunicipalTax.Entity;
using System;

namespace MunicipalTax.Common.Services
{
    public interface IMunicipalityTaxService
    {
        /// <summary>
        /// Get tax details for the required municipality for a given date.
        /// </summary>
        /// <param name="municipality"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        ResponseModel GetMunicipalityTax(string municipality, DateTime date);

        /// <summary>
        /// Add a municipality tax details.
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        ResponseModel AddMunicipalityTax(MunicipalityTax municipalityTaxRecord);

        /// <summary>
        /// Import bulk municipality tax details from a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        ResponseModel ImportBulkMunicipalityTaxRecords(string filePath);

        /// <summary>
        /// Update an existing municipality tax details.
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        ResponseModel UpdateMunicipalityTax(MunicipalityTax municipalityTaxRecord);
    }
}
