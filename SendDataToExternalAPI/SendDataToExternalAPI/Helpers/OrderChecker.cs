using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendDataToExternalAPI.Web.Helpers
{
    public class OrderChecker
    {
        private readonly Random random;
        private int index;

        private readonly string[] Status =
            {"Send request", "Still sending", "Half the way done", "Almost done", "Done!"};

        public OrderChecker(Random random)
        {
            this.random = random;
        }

        public CheckResult GetUpdate(int orderNo)
        {
            if (random.Next(1, 5) == 4)
            {
                if (Status.Length - 1 > index)
                {
                    index++;
                    var result = new CheckResult
                    {
                        New = true,
                        Update = Status[index],
                        Finished = Status.Length - 1 == index
                    };
                    return result;
                }
            }

            return new CheckResult { New = false };
        }
    }
    public class CheckResult
    {
        public bool New { get; set; }
        public string Update { get; set; }
        public bool Finished { get; set; }
    }
}
