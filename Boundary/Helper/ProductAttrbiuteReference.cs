using System.Collections.Generic;

namespace Boundary.Helper
{
    public class PAR
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
    public class ProductAttrbiuteReference
    {
        public static List<PAR> ProductAttrbiuteReferenceList=new List<PAR>()
        {
            DigitalMobileWithTabletPAR,DigitalBagWithCoverPAR,DigitalHeadphonesWithHandsfreePAR,
            DigitalCableWithConverterPAR,DigitalMemoryCardPAR,DigitalPowerBankPAR,
            DigitalLabtopPAR,DigitalFlashMemoryPAR,DigitalExternalHardPAR,DigitalCameraPAR,
            DigitalTVPAR,HomeApplianceWashingMachinePAR,HomeApplianceRefrigeratorPAR,
            HomeApplianceKitchenUtensilsPAR,ClothingPAR,BeautyAromaPAR,BeautyCosmeticPAR,
            BeautyWatchPAR,BeautyGoldWithJewelryPAR,BeautyGlassesPAR
        }; 


        #region static product attrbiute class

        public static PAR DigitalMobileWithTabletPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1001",
                    Title = "موبایل و تبلت"
                };
            }
        }

        public static PAR DigitalBagWithCoverPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1002",
                    Title = "کیف و کاور"
                };
            }
        }

        public static PAR DigitalHeadphonesWithHandsfreePAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1003",
                    Title = "هدفون و هندزفری"
                };
            }
        }

        public static PAR DigitalCableWithConverterPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1004",
                    Title = "کابل و مبدل"
                };
            }
        }

        public static PAR DigitalMemoryCardPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1005",
                    Title = "کارت حافظه"
                };
            }
        }

        public static PAR DigitalPowerBankPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1006",
                    Title = "پاور بانک"
                };
            }
        }

        public static PAR DigitalLabtopPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1007",
                    Title = "لب تاپ"
                };
            }
        }

        public static PAR DigitalFlashMemoryPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1008",
                    Title = "فلش"
                };
            }
        }

        public static PAR DigitalExternalHardPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1009",
                    Title = "هارد اکسترنال"
                };
            }
        }

        public static PAR DigitalCameraPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1010",
                    Title = "دوربین"
                };
            }
        }

        public static PAR DigitalTVPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "1011",
                    Title = "تلویزیون"
                };
            }
        }

        public static PAR HomeApplianceWashingMachinePAR
        {
            get
            {
                return new PAR()
                {
                    Id = "2012",
                    Title = "ماشین لباسشویی"
                };
            }
        }

        public static PAR HomeApplianceRefrigeratorPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "2013",
                    Title = "یخچال"
                };
            }
        }

        public static PAR HomeApplianceKitchenUtensilsPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "2014",
                    Title = "لوازم آشپزخانه"
                };
            }
        }

        public static PAR ClothingPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "4015",
                    Title = "پوشاک"
                };
            }
        }

        public static PAR BeautyAromaPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "3016",
                    Title = "عطر"
                };
            }
        }

        public static PAR BeautyCosmeticPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "3017",
                    Title = "لوازم آرایشی و بهداشتی"
                };
            }
        }

        public static PAR BeautyWatchPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "3018",
                    Title = "ساعت"
                };
            }
        }

        public static PAR BeautyGoldWithJewelryPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "3019",
                    Title = "طلا و زیورآلات"
                };
            }
        }

        public static PAR BeautyGlassesPAR
        {
            get
            {
                return new PAR()
                {
                    Id = "3020",
                    Title = "عینک"
                };
            }
        }

        #endregion

    }
}