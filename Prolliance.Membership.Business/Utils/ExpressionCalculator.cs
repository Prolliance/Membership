using TpNet;

namespace Prolliance.Membership.Business.Utils
{
    public static class ExpressionCalculator
    {

        static ExpressionCalculator()
        {
            //Tp.CodeBegin = "\\{\\%";
            //Tp.CodeEnd = "\\%\\}";
            //整个脚本上下文有效
            Tp.ScriptContext.SetParameterWithType("$User", typeof(User));
            Tp.ScriptContext.SetParameterWithType("$Role", typeof(Role));
            Tp.ScriptContext.SetParameterWithType("$Organization", typeof(Organization));
        }

        public static string Calculate(string expr, object model)
        {
            return Tp.Parse(expr, model) ?? string.Empty;
        }
    }
}
