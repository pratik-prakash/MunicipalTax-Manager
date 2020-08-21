using MunicipalTax.Common.DataModels;
using MunicipalTax.Entity;
using MunicipalTax.Entity.DataHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MunicipalTax.Common
{
    public static class Utility
    {
        /// <summary>
        /// Create success response.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static ResponseModel CreateSuccessResponse(object payload)
        {
            return new ResponseModel(true, null, payload);
        }

        /// <summary>
        /// Create error response.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ResponseModel CreateErrorResponse(string error)
        {
            return new ResponseModel(false, error, null);
        }

        /// <summary>
        /// Validate taxation schedule period.
        /// </summary>
        /// <param name="taxType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool ValidatePeriod(TaxType taxType, DateTime startDate, DateTime endDate)
        {
            bool _isValid = false;

            switch (taxType)
            {
                case TaxType.Daily:
                    if (startDate == endDate)
                        _isValid = true;
                    break;
                case TaxType.Weekly:
                    if (startDate.AddDays(6) == endDate)
                        _isValid = true;
                    break;
                case TaxType.Monthly:
                    if (startDate.AddMonths(1).AddDays(-1) == endDate)
                        _isValid = true;
                    break;
                case TaxType.Yearly:
                    if (startDate.AddYears(1).AddDays(-1) == endDate)
                        _isValid = true;
                    break;
                default:
                    break;
            }
            return _isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<MunicipalityTax> ParsePayFileContent(StreamReader reader)
        {
            var municipalityTaxRecords = new List<MunicipalityTax>();

            while (!reader.EndOfStream)
            {
                string dataLine = reader.ReadLine();
                if (!String.IsNullOrWhiteSpace(dataLine))
                {
                    municipalityTaxRecords.Add(GetMunicipalityTaxRecord(dataLine));
                }
            }

            return municipalityTaxRecords;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataLine"></param>
        /// <returns></returns>
        private static MunicipalityTax GetMunicipalityTaxRecord(string dataLine)
        {
            string[] dataValues = dataLine.Split(',');

            if (dataValues.Length <= 4)
            {
                throw new FormatException("File Data did not match the expected format: MunicipalityName, TaxType, Start date, End Date, Tax");
            }

            var municipalityTax = new MunicipalityTax
            {
                MunicipalityName = dataValues[0],
                TaxType = GetTaxType(dataValues[1]),
                StartDate = GetDateFromString(dataValues[2])
            };

            if (string.IsNullOrEmpty(dataValues[3]) && municipalityTax.TaxType == TaxType.Daily
                && municipalityTax.StartDate != DateTime.MinValue)
            {
                municipalityTax.EndDate = municipalityTax.StartDate;
            }
            else
            {
                municipalityTax.EndDate = GetDateFromString(dataValues[3]);
            }

            if (!ValidatePeriod(municipalityTax.TaxType, municipalityTax.StartDate, municipalityTax.EndDate))
            {
                throw new FormatException("Invalid Tax period schedule.");
            }

            municipalityTax.Tax = GetTaxFromString(dataValues[4]);

            return municipalityTax;
        }


        /// <summary>
        /// Get date type from a given date string.
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateTime GetDateFromString(string dateString)
        {
            if (!DateTime.TryParse(dateString, out DateTime convertedDate))
            {
                convertedDate = DateTime.MinValue;
            }
            return convertedDate;
        }

        /// <summary>
        /// Get Tax value from a given tax string.
        /// </summary>
        /// <param name="taxString"></param>
        /// <returns></returns>
        private static double GetTaxFromString(string taxString)
        {
            if (!double.TryParse(taxString, out double tax))
                throw new FormatException($"Tax value has an invalid format. Value:{taxString}");

            return tax;
        }

        /// <summary>
        /// Get TaxType from a given tax type string.
        /// </summary>
        /// <param name="taxTypeString"></param>
        /// <returns></returns>
        private static TaxType GetTaxType(string taxTypeString)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            taxTypeString = textInfo.ToTitleCase(taxTypeString.ToLower());

            if (!Enum.TryParse(taxTypeString, out TaxType taxType))
            {
                var acceptedValues = string.Join(",", Enum.GetValues(typeof(TaxType)).Cast<TaxType>());

                throw new FormatException($"Tax type value has an invalid format. Accepted values: {acceptedValues}. Value Received: {taxTypeString}");
            }
            return taxType;
        }
    }
}
