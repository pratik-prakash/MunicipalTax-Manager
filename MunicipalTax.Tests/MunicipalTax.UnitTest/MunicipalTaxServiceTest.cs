using Moq;
using MunicipalTax.Common;
using MunicipalTax.DAL.Interfaces;
using MunicipalTax.Entity;
using MunicipalTax.Services;
using System;
using Xunit;

namespace MunicipalTax.UnitTest
{
    public class MunicipalTaxServiceTest
    {
        private const string _municipalityName = "Bangalore";

        /// <summary>
        /// Daily tax test.
        /// </summary>
        [Fact]
        public void GetMunicipalityDailyTaxTest()
        {

            MunicipalityTax municipalityTax = new MunicipalityTax
            {
                MunicipalityTaxId = 1,
                MunicipalityName = _municipalityName,
                TaxType = Entity.DataHelper.TaxType.Daily,
                StartDate = Utility.GetDateFromString("2020.01.01"),
                EndDate = Utility.GetDateFromString("2020.01.01"),
                Tax = 0.1
            };

            //Mock the data access
            var _municipalityDataAccess = new Mock<IMunicipalityTaxDataAccess>();
            _municipalityDataAccess.Setup(mTax => mTax.GetMunicipalityTax(_municipalityName, Utility.GetDateFromString("2020.01.01"))).Returns(municipalityTax);

            Assert.Equal(municipalityTax.Tax, GetTaxRate(_municipalityDataAccess.Object, Utility.GetDateFromString("2020.01.01")));
        }

        /// <summary>
        /// Weekly tax test.
        /// </summary>
        [Fact]
        public void GetMunicipalityWeeklyTaxTest()
        {

            MunicipalityTax municipalityTax = new MunicipalityTax
            {
                MunicipalityTaxId = 2,
                MunicipalityName = _municipalityName,
                TaxType = Entity.DataHelper.TaxType.Weekly,
                StartDate = Utility.GetDateFromString("2020.03.10"),
                EndDate = Utility.GetDateFromString("2020.03.16"),
                Tax = 0.5
            };

            //Mock the data access
            var _municipalityDataAccess = new Mock<IMunicipalityTaxDataAccess>();
            _municipalityDataAccess.Setup(mTax => mTax.GetMunicipalityTax(_municipalityName, Utility.GetDateFromString("2016.03.14"))).Returns(municipalityTax);

            Assert.Equal(municipalityTax.Tax, GetTaxRate(_municipalityDataAccess.Object, Utility.GetDateFromString("2016.03.14")));
        }

        /// <summary>
        /// Monthly tax test.
        /// </summary>
        [Fact]
        public void GetMunicipalityMonthlyTaxTest()
        {

            MunicipalityTax municipalityTax = new MunicipalityTax
            {
                MunicipalityTaxId = 3,
                MunicipalityName = _municipalityName,
                TaxType = Entity.DataHelper.TaxType.Monthly,
                StartDate = Utility.GetDateFromString("2020.10.01"),
                EndDate = Utility.GetDateFromString("2020.10.31"),
                Tax = 0.4
            };

            //Mock the data access
            var _municipalityDataAccess = new Mock<IMunicipalityTaxDataAccess>();
            _municipalityDataAccess.Setup(mTax => mTax.GetMunicipalityTax(_municipalityName, Utility.GetDateFromString("2020.10.15"))).Returns(municipalityTax);

            Assert.Equal(municipalityTax.Tax, GetTaxRate(_municipalityDataAccess.Object, Utility.GetDateFromString("2020.10.15")));
        }

        /// <summary>
        /// Yearly tax test.
        /// </summary>
        [Fact]
        public void GetMunicipalityYearlyTaxTest()
        {

            MunicipalityTax municipalityTax = new MunicipalityTax
            {
                MunicipalityTaxId = 4,
                MunicipalityName = _municipalityName,
                TaxType = Entity.DataHelper.TaxType.Yearly,
                StartDate = Utility.GetDateFromString("2020.01.01"),
                EndDate = Utility.GetDateFromString("2020.12.31"),
                Tax = 0.2
            };

            //Mock the data access
            var _municipalityDataAccess = new Mock<IMunicipalityTaxDataAccess>();
            _municipalityDataAccess.Setup(mTax => mTax.GetMunicipalityTax(_municipalityName, Utility.GetDateFromString("2020.08.10"))).Returns(municipalityTax);

            Assert.Equal(municipalityTax.Tax, GetTaxRate(_municipalityDataAccess.Object, Utility.GetDateFromString("2020.08.10")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_municipalityDataAccess"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private double GetTaxRate(IMunicipalityTaxDataAccess _municipalityDataAccess, DateTime date)
        {
            var _municipalityTaxService = new MunicipalityTaxService(_municipalityDataAccess);

            var taxRate = (_municipalityTaxService.GetMunicipalityTax(_municipalityName, date).Payload as MunicipalityTax).Tax;

            return taxRate;
        }
    }
}
