using Appearition.API;

namespace Appearition.BuildingsAndFloors.API
{
    /// <summary>
    /// https://api.appearition.com/TenantName/api/Building/Create/0 , where 0 is Channel ID
    ///
    /// The post data is a Building object.
    /// </summary>
    [System.Serializable]
    public class Building_Create : BaseApiPost
    {
        public override int ApiVersion => 2;
        public override TypeOfApiStorage ResponseSaveType => TypeOfApiStorage.None;

        public Building Data;
    }
}