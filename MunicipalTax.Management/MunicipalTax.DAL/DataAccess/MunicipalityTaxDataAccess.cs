using MunicipalTax.DAL.Interfaces;
using MunicipalTax.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalTax.DAL.DataAccess
{
    public class MunicipalityTaxDataAccess : IMunicipalityTaxDataAccess
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MunicipalityTaxDataAccess()
        {

        }

        /// <summary>
        /// Add a municipality tax details.
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        public MunicipalityTax AddMunicipalityTaxRecord(MunicipalityTax municipalityTaxRecord)
        {
            using var municipalTaxContext = new MunicipalTaxContext();

            municipalTaxContext.MunicipalityTaxes.Add(municipalityTaxRecord);
            municipalTaxContext.SaveChanges();

            return municipalTaxContext.MunicipalityTaxes
                .FirstOrDefault(municipalityTax => municipalityTax.MunicipalityTaxId == municipalityTaxRecord.MunicipalityTaxId);
        }

        /// <summary>
        /// Import bulk municipality tax details from a file.
        /// </summary>
        /// <param name="municipalityTaxRecords"></param>
        /// <returns></returns>
        public bool ImportBulkMunticipalityTaxRecords(IEnumerable<MunicipalityTax> municipalityTaxRecords)
        {
            bool _success;
            try
            {
                using var municipalTaxContext = new MunicipalTaxContext();

                municipalTaxContext.MunicipalityTaxes.AddRange(municipalityTaxRecords);
                municipalTaxContext.SaveChanges();

                _success = true;
            }
            catch (Exception)
            {
                _success = false;
            }
            return _success;
        }

        /// <summary>
        /// Get municipality tax details by name.
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <returns></returns>
        public List<MunicipalityTax> GetMunicipalityTaxDetailsByName(string municipalityName)
        {
            using var municipalTaxContext = new MunicipalTaxContext();

            return municipalTaxContext.MunicipalityTaxes.Where(mTax => mTax.MunicipalityName == municipalityName).ToList();
        }

        /// <summary>
        /// Get tax details for the required municipality for a given date.
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public MunicipalityTax GetMunicipalityTax(string municipalityName, DateTime date)
        {
            using var municipalTaxContext = new MunicipalTaxContext();

            return municipalTaxContext.MunicipalityTaxes.Where(tax => tax.MunicipalityName == municipalityName && tax.StartDate <= date && tax.EndDate >= date)
                .OrderBy(tax => tax.TaxType)
                .FirstOrDefault();
        }

        /// <summary>
        ///  Update an existing municipality tax details.
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        public MunicipalityTax UpdateMunicipalityTax(MunicipalityTax municipalityTaxRecord)
        {
            using var municipalTaxContext = new MunicipalTaxContext();
            municipalTaxContext.MunicipalityTaxes.Update(municipalityTaxRecord);
            municipalTaxContext.SaveChanges();

            return municipalityTaxRecord;
        }
    }
}
