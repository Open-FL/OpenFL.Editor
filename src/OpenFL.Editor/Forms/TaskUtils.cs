using System;
using System.Linq;

namespace OpenFL.Editor.Forms
{
    public static class TaskUtils
    {

        public static Exception GetInnerIfAggregate(Exception ex)
        {
            return ex is AggregateException ag ? GetInnerIfAggregate(ag.InnerExceptions.First()) : ex;
        }

    }
}