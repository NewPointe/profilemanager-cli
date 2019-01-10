using System;
using System.Text;

namespace org.newpointe.util
{
    public static class CliParseUtil
    {
        public static string TryShiftString(string[] args, out string[] outArgs)
        {
            if(args.Length == 0) {
                outArgs = args;
                return null;
            }
            else {
                outArgs = ArrayUtil.Shift(args);
                return args[0];
            }
        }
        public static Nullable<int> TryShiftInt(string[] args, out string[] outArgs)
        {
            if(args.Length == 0) {
                outArgs = args;
                return null;
            }
            else {
                if(int.TryParse(args[0], out int rtnVal)) {
                    outArgs = ArrayUtil.Shift(args);
                    return rtnVal;
                }
                else {
                    outArgs = args;
                    return null;
                }
            }
        }
        public static Nullable<int> TryShiftId(string[] args, out string[] outArgs)
        {
            if(args.Length == 0) {
                outArgs = args;
                return null;
            }
            else {
                if(int.TryParse(args[0], out int rtnVal) && rtnVal > 0) {
                    outArgs = ArrayUtil.Shift(args);
                    return rtnVal;
                }
                else {
                    outArgs = args;
                    return null;
                }
            }
        }
    }
}