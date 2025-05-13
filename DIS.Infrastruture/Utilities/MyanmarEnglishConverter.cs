using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastruture.Utilities
{
    public static class MyanmarEnglishConverter
    {
        public static string ToMyanmarNumber(string englishNumber)
        {
            if (!string.IsNullOrEmpty(englishNumber))
            {
                return englishNumber.ToString().Replace("0", "၀").Replace("1", "၁").Replace("2", "၂").Replace("3", "၃").Replace("4", "၄").Replace("5", "၅")
                                     .Replace("6", "၆").Replace("7", "၇").Replace("8", "၈").Replace("9", "၉");
            }
            else
            {
                return englishNumber;
            }

        }

        public static string ToEnglishNumber(string myanmarNumber)
        {
            if (!string.IsNullOrEmpty(myanmarNumber))
            {
                return myanmarNumber.ToString().Replace("၀", "0").Replace("၁", "1").Replace("၂", "2").Replace("၃", "3").Replace("၄", "4").Replace("၅", "5")
                                     .Replace("၆", "6").Replace("၇", "7").Replace("၈", "8").Replace("၉", "9");
            }
            else
            {
                return myanmarNumber;
            }

        }
    }
}
