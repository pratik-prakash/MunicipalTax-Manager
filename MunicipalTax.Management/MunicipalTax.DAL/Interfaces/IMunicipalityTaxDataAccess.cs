using MunicipalTax.Entity;
using System;
using System.Collections.Generic;

namespace MunicipalTax.DAL.Interfaces
{
    public interface IMunicipalityTaxDataAccess
    {
        /// <summary>
        /// Get municipality tax details by name.
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <returns></returns>
        List<MunicipalityTax> GetMunicipalityTaxDetailsByName(string municipalityName);

        /// <summary>
        /// Get tax details for the required municipality for a given date.
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        MunicipalityTax GetMunicipalityTax(string municipalityName, DateTime date);

        /// <summary>
        /// Add a municipality tax details.
        /// </summary>
        /// <param name="municipalityTax"></param>
        /// <returns></returns>
        MunicipalityTax AddMunicipalityTaxRecord(MunicipalityTax municipalityTaxRecord);

        /// <summary>
        /// Import bulk municipality tax details from a file.
        /// </summary>
        /// <param name="municipalityTaxRecords"></param>
        /// <returns></returns>
        bool ImportBulkMunticipalityTaxRecords(IEnumerable<MunicipalityTax> municipalityTaxRecords);

        /// <summary>
        /// Update an existing municipality tax details.
        /// </summary>
        /// <param name="municipalityTax"></param>
        /// <returns></returns>
        MunicipalityTax UpdateMunicipalityTax(MunicipalityTax municipalityTaxRecord);
    }
}
