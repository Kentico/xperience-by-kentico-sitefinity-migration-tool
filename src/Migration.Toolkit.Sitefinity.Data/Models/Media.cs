using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Models
{
    public class Media : MediaDto, ISitefinityModel
    {
        public new Guid Id
        {
            get
            {
                if (Guid.TryParse(base.Id, out var id))
                {
                    return id;
                }

                return Guid.Empty;
            }
            set => base.Id = value.ToString();
        }

        public DateTime LastModified { get; set; }

        public Guid? LastModifiedBy { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
