using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public static class Validator
    {
        public static void NoEmptyItem(ValidationContext context, string key, IEnumerable<string> list)
        {
            context.PushPath(key);

            int index = 0;

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    context.Require(InvariantString.Format("[{0}]", index));
                }

                index++;
            }

            context.PopPath();
        }
    }
}
