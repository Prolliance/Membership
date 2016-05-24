
namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// Ȩ�޶����ģ�ͣ�Ψһ������AppKey + Code��
    /// </summary>
    public class TargetInfo : IModel
    {
        public string Id { get; set; }
        public string AppId { get; set; }
        public string Code { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Scope { get; set; }
        public int Sort { get; set; }
    }
}
