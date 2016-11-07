using Kancelaria.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kancelaria.Tests.Controllers
{
    [TestClass]
    public class LataObrotoweRepositoryTest
    {
        [TestMethod]
        public void ShouldBeAbleToCallMultipleTimesRepository()
        {
            var initialRepository = new LataObrotoweRepository();

            int ustawianeIdRoku = 22;

            initialRepository.WybierzIdRoku(ustawianeIdRoku, "vn");
            initialRepository.Save();

            for (int i = 0; i < 30; i++)
            {
                var repository = new LataObrotoweRepository();
                (new TaskFactory()).StartNew(() =>
                {
                    repository.WybierzIdRoku(ustawianeIdRoku, "vn");
                    repository.Save();
                });
                var odczytaneIdRoku = (new LataObrotoweRepository()).WybraneIdRokuOrExcepion("vn");

                Assert.IsTrue(ustawianeIdRoku == odczytaneIdRoku, String.Format("Rozne id roku dla {0} wywolania ({1} != {2})", i, ustawianeIdRoku, odczytaneIdRoku));

                //if (i % 3 == 0)
                //{
                //    ustawianeIdRoku = 17;
                //}
                //else
                //if (i % 3 == 1)
                //{
                //    ustawianeIdRoku = 22;
                //}
                //else
                //if (i % 3 == 2)
                //{
                //    ustawianeIdRoku = 23;
                //}
            }
        }
    }
}
