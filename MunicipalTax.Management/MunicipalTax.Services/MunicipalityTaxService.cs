using MunicipalTax.Common;
using MunicipalTax.Common.DataModels;
using MunicipalTax.Common.Services;
using MunicipalTax.DAL.Interfaces;
using MunicipalTax.Entity;
using System;
using System.IO;
using System.Linq;

namespace MunicipalTax.Services
{
    public class MunicipalityTaxService : IMunicipalityTaxService
    {
        private readonly IMunicipalityTaxDataAccess _municipalityDataAccess;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="municipalityDataAccess"></param>
        public MunicipalityTaxService(IMunicipalityTaxDataAccess municipalityDataAccess)
        {
            _municipalityDataAccess = municipalityDataAccess;
        }

        /// <summary>
        /// Add a municipality tax details.
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        public ResponseModel AddMunicipalityTax(MunicipalityTax municipalityTaxRecord)
        {
            try
            {
                if (municipalityTaxRecord.TaxType == Entity.DataHelper.TaxType.Daily && municipalityTaxRecord.StartDate != DateTime.MinValue
                    && municipalityTaxRecord.EndDate == DateTime.MinValue)
                {
                    municipalityTaxRecord.EndDate = municipalityTaxRecord.StartDate;
                }

                if (!string.IsNullOrEmpty(municipalityTaxRecord.MunicipalityName)
                    && Utility.ValidatePeriod(municipalityTaxRecord.TaxType, municipalityTaxRecord.StartDate, municipalityTaxRecord.EndDate))
                {
                    var municipalityTaxList = _municipalityDataAccess.GetMunicipalityTaxDetailsByName(municipalityTaxRecord.MunicipalityName);

                    if (municipalityTaxList != null && municipalityTaxList.Count > 0)
                    {
                        var existingTaxDetails = municipalityTaxList.Where(mTax => mTax.StartDate == municipalityTaxRecord.StartDate
                       && mTax.TaxType == municipalityTaxRecord.TaxType).FirstOrDefault();

                        if (existingTaxDetails != null)
                        {
                            return Utility.CreateErrorResponse("Municipality tax details already present for the given period and start date.");
                        }
                        else
                        {
                            return Utility.CreateSuccessResponse(_municipalityDataAccess.AddMunicipalityTaxRecord(municipalityTaxRecord));
                        }
                    }
                    else
                    {
                        return Utility.CreateSuccessResponse(_municipalityDataAccess.AddMunicipalityTaxRecord(municipalityTaxRecord));
                    }
                }
                else
                {
                    return Utility.CreateErrorResponse("Invalid parameters.");
                }

            }
            catch (Exception ex)
            {
                //TODO: Log the exception.
                return Utility.CreateErrorResponse("Something went wrong.");
            }

        }

        /// <summary>
        /// Get tax details for the required municipality for a given date.
        /// </summary>
        /// <param name="municipality"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseModel GetMunicipalityTax(string municipalityName, DateTime date)
        {
            try
            {
                if (!string.IsNullOrEmpty(municipalityName) && date != DateTime.MinValue)
                {
                    var mTaxDetails = _municipalityDataAccess.GetMunicipalityTax(municipalityName, date);

                    if (mTaxDetails != null)
                    {
                        return Utility.CreateSuccessResponse(mTaxDetails);
                    }
                    else
                    {
                        return Utility.CreateSuccessResponse("No tax details present for the given municipality and date.");
                    }

                }
                else
                {
                    return Utility.CreateErrorResponse("Invalid parameters.");
                }
            }
            catch (Exception)
            {
                //TODO: Log the exception.
                return Utility.CreateErrorResponse("Something went wrong.");
            }
        }

        /// <summary>
        /// Import bulk municipality tax details from a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ResponseModel ImportBulkMunicipalityTaxRecords(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(Path.GetFullPath(filePath)))
                {
                    var municipalityTaxRecords = Utility.ParsePayFileContent(reader);

                    if (municipalityTaxRecords != null && municipalityTaxRecords.Count() > 0)
                    {
                        return (Utility.CreateSuccessResponse(_municipalityDataAccess.ImportBulkMunticipalityTaxRecords(municipalityTaxRecords)));
                    }
                    else
                    {
                        return Utility.CreateErrorResponse("Something went wrong.");
                    }

                }
            }
            catch (FormatException ex)
            {
                return Utility.CreateErrorResponse(ex.Message);
            }

            catch (Exception)
            {
                //TODO: Log the exception.
                return Utility.CreateErrorResponse("Something went wrong.");
            }
        }

        /// <summary>
        /// Update an existing municipality tax details.
        /// </summary>
        /// <param name="municipalityTaxRecord"></param>
        /// <returns></returns>
        public ResponseModel UpdateMunicipalityTax(MunicipalityTax municipalityTaxRecord)
        {
            try
            {
                if (!string.IsNullOrEmpty(municipalityTaxRecord.MunicipalityName))
                {
                    var municipalityTaxList = _municipalityDataAccess.GetMunicipalityTaxDetailsByName(municipalityTaxRecord.MunicipalityName);

                    if (municipalityTaxList != null && municipalityTaxList.Count > 0)
                    {
                        var existingTaxDetails = municipalityTaxList.Where(mTax => mTax.TaxType == municipalityTaxRecord.TaxType
                        || mTax.StartDate == municipalityTaxRecord.StartDate).FirstOrDefault();

                        if (existingTaxDetails != null)
                        {
                            existingTaxDetails.MunicipalityName = municipalityTaxRecord.MunicipalityName;

                            if (municipalityTaxRecord.Tax > 0)
                            {
                                existingTaxDetails.Tax = municipalityTaxRecord.Tax;
                            }

                            if (municipalityTaxRecord.StartDate != DateTime.MinValue || municipalityTaxRecord.EndDate != DateTime.MinValue)
                            {
                                if (Utility.ValidatePeriod(municipalityTaxRecord.TaxType, municipalityTaxRecord.StartDate, municipalityTaxRecord.EndDate))
                                {
                                    existingTaxDetails.TaxType = municipalityTaxRecord.TaxType;
                                    existingTaxDetails.StartDate = municipalityTaxRecord.StartDate;
                                    existingTaxDetails.EndDate = municipalityTaxRecord.EndDate;
                                }
                            }

                            return Utility.CreateSuccessResponse(_municipalityDataAccess.UpdateMunicipalityTax(existingTaxDetails));
                        }
                        else
                        {
                            return Utility.CreateErrorResponse("No existing municipality tax details present for the given period or start date.");
                        }
                    }
                    else
                    {
                        return Utility.CreateErrorResponse("No existing municipality tax details present for the given period or start date.");
                    }
                }
                else
                {
                    return Utility.CreateErrorResponse("Invalid parameters.");
                }
            }
            catch (Exception)
            {
                //TODO: Log the exception.
                return Utility.CreateErrorResponse("Something went wrong.");
            }
        }
    }
}
